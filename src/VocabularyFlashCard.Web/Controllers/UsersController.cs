namespace VocabularyFlashCard.Web.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
	private readonly IAppUserService _appUserService;

	public UsersController(IAppUserService appUserService)
	{
		_appUserService = appUserService;
	}


	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult> GetAsync()
	{
		var users = await _appUserService.Users();
		return Ok(users);
	}


	[HttpGet("Id")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult> GetByIdAsync(string id)
	{
		if (!string.IsNullOrWhiteSpace(id))
		{
			var userViewModel = await _appUserService.UserById(id);
			if (userViewModel != default)
			{
				return Ok(userViewModel);
			}
		}
		return NotFound();
	}


	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> AddEdit(AppUserViewModel appUserViewModel)
	{
		appUserViewModel.TrimAllStrings();
		await _appUserService.CheckCustomValidationAsync(appUserViewModel, ModelState);
		await _appUserService.SaveAsync(appUserViewModel);
		return Created(string.Empty, appUserViewModel);
	}
}
