using VocabularyFlashCard.Core.Internal;

namespace VocabularyFlashCard.Core.ViewModels;

public class VocabularyReminderViewModel
{
	public int VocabularyId { get; set; } = default;
	public bool KnowIt { get; set; } = false;

	[Range(1, AppConstants.MaxGroup,
		ErrorMessage = "Value for {0} must be between {1} and {2}")]
	public int NextVocabularyGroupId { get; set; } = 1;
}
