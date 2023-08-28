namespace VocabularyFlashCard.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
	private readonly IAuthService _authService;
	private readonly ILogger<AuthController> _logger;

	public AuthController(
		IAuthService authService,
		ILogger<AuthController> logger)
	{
		_authService = authService;
		_logger = logger;
	}


	[HttpPost("Login")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<object> ApiLogIn(LoginViewModel loginViewModel)
	{
		loginViewModel.TrimAllStrings();

		var authViewModel = await _authService.Login(loginViewModel);
		if (authViewModel.IsLoggedIn)
		{
			return authViewModel;
		}
		else
		{
			_logger.LogWarning("Failed login attempt: {loginViewModel}", JsonSerializer.Serialize(loginViewModel));
		}

		return Unauthorized();
	}


	[HttpPost("Logout")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> ApiLogOut()
	{
		await _authService.Logout();
		return Ok();
	}
}

