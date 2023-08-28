namespace VocabularyFlashCard.DataService.Services.Interfaces;

public interface IVocabularyService
{
	public Task<VocabularyListViewModel> VocabulariesAsync(string? searchPhrase, int? page, int? itemsPerPage);
	public Task<DifficultVocabulariesViewModel> DifficultMarkedVocabularies();
	public Task<VocabularyMarkedViewModel> ToggleVocabularyMarkAsync(int id);
	public Task SaveAsync(VocabularyWithMediaViewModel vocabularyWithMedia);
}
