namespace VocabularyFlashCard.DataService.Services.Interfaces;

public interface IVocabularyMediaService
{
	public Task<byte[]> VocabularyMediumFileDataAsync(int id);
	public Task SaveMediaAsync(List<VocabularyMedia> vocabularyMedia, int receivedVocabularyId);
}
