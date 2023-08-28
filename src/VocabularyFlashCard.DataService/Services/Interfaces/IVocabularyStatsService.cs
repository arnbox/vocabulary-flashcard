namespace VocabularyFlashCard.DataService.Services.Interfaces;

public interface IVocabularyStatsService
{
	public Task<StatsViewModel> GetStats();
}
