namespace VocabularyFlashCard.Web.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("[controller]")]
public class VocabulariesController : ControllerBase
{
	private readonly IVocabularyService _vocabularyService;

	public VocabulariesController(IVocabularyService vocabularyService)
	{
		_vocabularyService = vocabularyService;
	}


	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult> Get(
		string? searchPhrase,
		int? page,
		int? itemsPerPage)
	{
		var vocabularies = await _vocabularyService.VocabulariesAsync(searchPhrase, page, itemsPerPage);
		return Ok(vocabularies);

	}


	[HttpPost]
	[ProducesResponseType(StatusCodes.Status202Accepted)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> AddEdit(VocabularyWithMediaViewModel vocabularyWithMedia)
	{
		vocabularyWithMedia.TrimAllStrings();
		await _vocabularyService.SaveAsync(vocabularyWithMedia);

		return Accepted(string.Empty, vocabularyWithMedia);
	}


	[HttpPost(nameof(MarkVocabulary) + "/{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult> MarkVocabulary(int? id)
	{
		if (id.HasValue)
		{
			var vocMarked = await _vocabularyService.ToggleVocabularyMarkAsync(id.Value);
			if (vocMarked != default(VocabularyMarkedViewModel))
			{
				return Ok(vocMarked);
			}
		}

		return BadRequest();
	}


	[HttpGet(nameof(DifficultVocabularies))]
	[ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult> DifficultVocabularies()
	{
		var vocs = await _vocabularyService.DifficultMarkedVocabularies();
		return Ok(vocs);
	}
}
