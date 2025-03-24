using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WorkTimeReport.Infrastructure.Extensions;

namespace VocabularyFlashCard.DataService.Services.VocabularyServices;

public class AuthService : IAuthService
{
	private readonly SignInManager<AppUser> _signInManager;
	private readonly UserManager<AppUser> _userManager;
	private readonly JwtOptions _jwtOptions;

	public AuthService(
			SignInManager<AppUser> signMgr,
			UserManager<AppUser> usrMgr,
			IOptions<JwtOptions> jwtOptions)
	{
		_signInManager = signMgr;
		_userManager = usrMgr;
		_jwtOptions = jwtOptions.Value;

	}

	public async Task<AuthViewModel> Login(LoginViewModel loginViewModel)
	{
		var authViewModel = new AuthViewModel { IsLoggedIn = false };
		var user = await _userManager.FindByEmailAsync(loginViewModel.Email);

		if (user is not null)
		{
			SignInResult result =
				await _signInManager.CheckPasswordSignInAsync(user, loginViewModel.Password, true);

			if (result.Succeeded)
			{
				var securityKey = Encoding.UTF8.GetBytes(_jwtOptions.Key);
				var email = user.Email ?? string.Empty;

				var claims = new Claim[]
				{
					new (JwtRegisteredClaimNames.Email, email),
					new (JwtRegisteredClaimNames.NameId, user.UserName ?? string.Empty),
				};
				var now = DateTime.Now.ToCustomeTimeZone();
				var jwtSecurityToken = new JwtSecurityToken
				(
					issuer: _jwtOptions.Issuer,
					audience: _jwtOptions.Audience,
					claims: claims,
					expires: now.AddMinutes(_jwtOptions.ExpiryInMinutes),
					signingCredentials: new SigningCredentials(new SymmetricSecurityKey(securityKey), SecurityAlgorithms.HmacSha512Signature)
				);

				var jwtToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
				authViewModel.IsLoggedIn = true;
				authViewModel.Email = email;
				authViewModel.Id = user.Id ?? string.Empty;
				authViewModel.Bearer = jwtToken;
			}
		}
		return authViewModel;
	}

	public async Task Logout()
	{
		await _signInManager.SignOutAsync(); // No signout for jwt
	}
}
