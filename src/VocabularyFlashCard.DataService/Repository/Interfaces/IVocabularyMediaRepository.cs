namespace VocabularyFlashCard.DataService.Repository.Interfaces;

public interface IVocabularyMediaRepository
{
	IQueryable<VocabularyMedia> VocabularyMedia { get; }
	Task AddAsync(VocabularyMedia vocabularyMedia);
	Task UpdateAsync(VocabularyMedia vocabularyMedia);
	Task DeleteAsync(VocabularyMedia vocabularyMedia);
}
