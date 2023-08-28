namespace VocabularyFlashCard.Web.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("[controller]")]
public class VocabularyReminderController : ControllerBase
{
	private readonly IVocabularyReminderService _vocabularyReminderService;

	public VocabularyReminderController(
		IVocabularyReminderService vocabularyReminderService)
	{
		_vocabularyReminderService = vocabularyReminderService;
	}


	[HttpPost(nameof(NextVocabulary))]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult> NextVocabulary(VocabularyReminderViewModel vocabularyReminderViewModel)
	{
		var voc = await _vocabularyReminderService.NextVocabulary(vocabularyReminderViewModel);

		if (voc == default(VocabularyViewModel))
		{
			return BadRequest();
		}

		return Ok(voc);
	}
}