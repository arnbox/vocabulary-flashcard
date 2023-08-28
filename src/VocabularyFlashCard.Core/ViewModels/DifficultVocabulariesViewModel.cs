namespace VocabularyFlashCard.Core.ViewModels;

public class DifficultVocabulariesViewModel
{
	public List<VocabularyViewModel> DifficultVocabularies { get; set; } = new List<VocabularyViewModel>();
	public List<VocabularyViewModel> MarkedVocabularies { get; set; } = new List<VocabularyViewModel>();
	public List<VocabularyViewModel> LastReadVocabularies { get; set; } = new List<VocabularyViewModel>();
}
