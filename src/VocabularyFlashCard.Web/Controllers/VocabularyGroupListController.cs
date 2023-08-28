namespace VocabularyFlashCard.Web.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("[controller]")]
public class VocabularyGroupListController : ControllerBase
{
	private readonly IVocabularyGroupListService _vocabularyGroupListService;

	public VocabularyGroupListController(IVocabularyGroupListService vocabularyGroupListService)
	{
		_vocabularyGroupListService = vocabularyGroupListService;
	}


	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult> GroupList()
	{
		var groupList = await _vocabularyGroupListService.GetGroupList();
		return Ok(groupList);
	}
}