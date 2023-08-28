using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace VocabularyFlashCard.DataService.Services;

public class AppUserService : IAppUserService
{
	private readonly UserManager<AppUser> _userManager;
	private readonly IUserValidator<AppUser> _userValidator;
	private readonly IPasswordValidator<AppUser> _passwordValidator;
	private readonly IPasswordHasher<AppUser> _passwordHasher;

	public AppUserService(UserManager<AppUser> userManager,
						  IUserValidator<AppUser> userValidator,
						  IPasswordValidator<AppUser> passValidator,
						  IPasswordHasher<AppUser> passwordHasher)
	{
		_userManager = userManager;
		_userValidator = userValidator;
		_passwordValidator = passValidator;
		_passwordHasher = passwordHasher;
	}

	public async Task<List<AppUserViewModel>> Users()
	{
		var users = await _userManager.Users.Select(user => new AppUserViewModel
		{
			Id = user.Id,
			UserName = user.UserName ?? string.Empty,
			Email = user.Email ?? string.Empty,
		}).OrderBy(u => u.UserName).ToListAsync();

		return users;
	}

	public async Task<AppUserViewModel> UserById(string id)
	{
		var appUser = await _userManager.FindByIdAsync(id);
		var userViewModel = new AppUserViewModel();

		if (appUser != null)
		{
			userViewModel = new AppUserViewModel
			{
				Id = appUser.Id,
				UserName = appUser.UserName ?? string.Empty,
				Email = appUser.Email ?? string.Empty,
			};
		}

		return userViewModel;
	}


	public async Task<AppUserViewModel> SaveAsync(AppUserViewModel appUserViewModel)
	{
		AppUser? user = null;

		if (!string.IsNullOrWhiteSpace(appUserViewModel.Id))
		{
			user = await _userManager.FindByIdAsync(appUserViewModel.Id);
		}

		if (user != null)   // edit user
		{
			assignUserViewModelToUser(user, appUserViewModel);
			if (!string.IsNullOrEmpty(appUserViewModel.Password))
			{
				// Update password if exist
				IdentityResult validPass = await _passwordValidator.ValidateAsync(_userManager, user, appUserViewModel.Password);
				if (validPass.Succeeded)
				{
					user.PasswordHash = _passwordHasher.HashPassword(user, appUserViewModel.Password);
				}
			}

			await _userManager.UpdateAsync(user);
		}
		else
		{
			/* new user */
			user = new AppUser();
			assignUserViewModelToUser(user, appUserViewModel);
			await _userManager.CreateAsync(user, appUserViewModel.Password);
		}

		return appUserViewModel;
	}

	public async Task CheckCustomValidationAsync(AppUserViewModel userViewModel, ModelStateDictionary modelState)
	{
		// UserName
		if (modelState.GetValidationState("UserName") == ModelValidationState.Valid
			&& userViewModel.UserName.Any(Char.IsWhiteSpace))
		{
			modelState.AddModelError("UserName", "Username cannot contain whitespace");
		}

		// Password
		if (modelState.GetValidationState("Password") == ModelValidationState.Valid
			&& string.IsNullOrWhiteSpace(userViewModel.Id)  // New user
			&& string.IsNullOrWhiteSpace(userViewModel.Password))
		{
			modelState.AddModelError("Password", "Password required for new user");
		}


		var appUser = await _userManager.FindByIdAsync(userViewModel.Id);

		if (appUser is not null)
		{
			// Identity password validator
			IdentityResult? validPass = null;
			if (!string.IsNullOrEmpty(userViewModel.Password))
			{
				validPass = await _passwordValidator.ValidateAsync(_userManager, appUser, userViewModel.Password);
				if (validPass.Succeeded)
				{
					appUser.PasswordHash = _passwordHasher.HashPassword(appUser, userViewModel.Password);
				}
				else
				{
					modelState.AddModelError("Password", validPass.Errors.First().Description);
				}
			}

			// Email Validator
			IdentityResult validEmail = await _userValidator.ValidateAsync(_userManager, appUser);
			if (!validEmail.Succeeded)
			{
				modelState.AddModelError("Email", validEmail.Errors.First().Description);
			}

		}
	}

	private static void assignUserViewModelToUser(AppUser user, AppUserViewModel appUserViewModel)
	{
		user.UserName = appUserViewModel.UserName;
		user.Email = appUserViewModel.Email;
	}
}
