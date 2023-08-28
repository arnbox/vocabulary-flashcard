namespace VocabularyFlashCard.Core.ViewModels;

public class VocabularyListViewModel
{
	public List<VocabularyViewModel> Vocabularies { get; set; } = new List<VocabularyViewModel>();
	//public List<VocabularyViewModel> MarkedVocabularys { get; set; }
	public PagingInfo PagingInfo { get; set; } = new PagingInfo();
}
