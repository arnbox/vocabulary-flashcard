namespace VocabularyFlashCard.DataService.Repository.EntityFramework;

public class EFVocabularyRepository : IVocabularyRepository
{
	private readonly AppDbContext _context;

	public EFVocabularyRepository(AppDbContext context)
	{
		_context = context;
	}

	public IQueryable<Vocabulary> Vocabularies => _context.Vocabularys;

	public async Task AddAsync(Vocabulary v)
	{
		await _context.AddAsync(v);
		await _context.SaveChangesAsync();
	}

	public async Task UpdateAsync(Vocabulary v)
	{
		_context.Update(v);
		await _context.SaveChangesAsync();
	}

	public async Task<int> SaveAsync(Vocabulary vocabulary)
	{
		// Check if it is new by Id of vocabulary
		if (vocabulary.VocabularyId == default)
		{
			vocabulary.VocabularyId = NewVocabularyId;
			await AddAsync(vocabulary);
		}
		else
		{
			await UpdateAsync(vocabulary);
		}

		return vocabulary.VocabularyId;
	}

	public int NewVocabularyId => (_context.Vocabularys?.Max(v => v.VocabularyId) + 1 ?? 1);


	public AppDbContext GetContext() => _context;
}
