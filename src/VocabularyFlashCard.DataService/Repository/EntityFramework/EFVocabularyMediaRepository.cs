namespace VocabularyFlashCard.DataService.Repository.EntityFramework;

public class EFVocabularyMediaRepository : IVocabularyMediaRepository
{
	private readonly AppDbContext _context;

	public EFVocabularyMediaRepository(AppDbContext context)
	{
		_context = context;
	}

	public IQueryable<VocabularyMedia> VocabularyMedia => _context.VocabularyMedia;

	public async Task AddAsync(VocabularyMedia vocabularyMedia)
	{
		vocabularyMedia.VocabularyMediaId = NewVocabularyMediaID;

		await _context.AddAsync(vocabularyMedia);
		await _context.SaveChangesAsync();
	}

	public async Task UpdateAsync(VocabularyMedia vocabularyMedia)
	{
		_context.Update<VocabularyMedia>(vocabularyMedia);
		await _context.SaveChangesAsync();
	}

	public async Task DeleteAsync(VocabularyMedia vocabularyMedia)
	{
		_context.Remove(vocabularyMedia);
		await _context.SaveChangesAsync();
	}

	int NewVocabularyMediaID => _context.VocabularyMedia?.Max(m => m.VocabularyMediaId) + 1 ?? 1;
}
