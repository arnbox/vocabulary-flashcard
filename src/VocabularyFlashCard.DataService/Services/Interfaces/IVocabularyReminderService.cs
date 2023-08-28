namespace VocabularyFlashCard.DataService.Services.Interfaces;

public interface IVocabularyReminderService
{
	public Task<VocabularyViewModel> NextVocabulary(VocabularyReminderViewModel vocabularyReminderViewModel);
}
