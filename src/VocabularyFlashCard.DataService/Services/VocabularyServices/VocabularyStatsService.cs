namespace VocabularyFlashCard.DataService.Services;
public class VocabularyStatsService : IVocabularyStatsService
{
	private readonly IVocabularyRepository _vocabularyRepository;

	public VocabularyStatsService(IVocabularyRepository vocabularyRepository)
	{
		_vocabularyRepository = vocabularyRepository;
	}

	public async Task<StatsViewModel> GetStats()
	{
		var voc = _vocabularyRepository.Vocabularies.Select(VocabularyServiceBuilder.VocabularyViewModelSelector);

		var orderedGroupsQuery = voc
			.GroupBy(v => v.GroupId)
			.OrderBy(g => g.Key)
			.Select(g => new GroupCount
			{
				Group = g.Key,
				Count = g.Count(),
				LastRead = g.Min(v => v.LastRead)
			});

		var stat = new StatsViewModel
		{
			TotalVocab = await voc.CountAsync(),
			NewWords = await voc.CountAsync(v => v.NumRepeat == AppConstants.NoRepeat),
			GroupCount = await orderedGroupsQuery.ToListAsync(),
			LongestWord = await voc.OrderByDescending(v => v.Word.Length)
			.FirstOrDefaultAsync() ?? new VocabularyViewModel(),

			LongestDefinition = await voc.OrderByDescending(v => v.Definition.Length)
			.FirstOrDefaultAsync() ?? new VocabularyViewModel(),

			Oldest = await voc.OrderBy(v => v.LastRead)
			.FirstOrDefaultAsync() ?? new VocabularyViewModel()
		};

		return stat;
	}
}
