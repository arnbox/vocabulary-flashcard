namespace VocabularyFlashCard.Web.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("[controller]")]
public class VocabularyStatsController : ControllerBase
{
	private readonly IVocabularyStatsService _vocabularyStatsService;

	public VocabularyStatsController(IVocabularyStatsService vocabularyStatsService)
	{
		_vocabularyStatsService = vocabularyStatsService;
	}


	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult> Stats()
	{
		var stats = await _vocabularyStatsService.GetStats();
		return Ok(stats);
	}
}