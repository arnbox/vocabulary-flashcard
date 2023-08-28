namespace VocabularyFlashCard.Web.Controllers;

// No Authorization is needed it's public
// [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("[controller]")]
public class VocabularyMediaController : ControllerBase
{
	private readonly IVocabularyMediaService _vocabularyMediaService;

	public VocabularyMediaController(IVocabularyMediaService vocabularyMediaService)
	{
		_vocabularyMediaService = vocabularyMediaService;
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult> GetMedia(int? id)
	{
		if (id.HasValue)
		{
			var medium = await _vocabularyMediaService.VocabularyMediumFileDataAsync(id.Value);
			if (medium.Any())
			{
				return File(medium, "audio/mpeg");
			}
		}

		return NotFound();
	}
}