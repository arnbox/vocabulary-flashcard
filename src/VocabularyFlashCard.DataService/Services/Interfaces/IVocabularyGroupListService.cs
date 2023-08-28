namespace VocabularyFlashCard.DataService.Services.Interfaces;

public interface IVocabularyGroupListService
{
	public Task<GroupListViewModel> GetGroupList();
}
