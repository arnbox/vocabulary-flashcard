namespace VocabularyFlashCard.DataService.Repository.Interfaces;

public interface IVocabularyRepository
{
	IQueryable<Vocabulary> Vocabularies { get; }

	Task AddAsync(Vocabulary v);
	Task UpdateAsync(Vocabulary v);
	Task<int> SaveAsync(Vocabulary v);  // Add or Update automatically
	AppDbContext GetContext();
	int NewVocabularyId { get; }

	//		void Delete(Vocabulary v);
}
