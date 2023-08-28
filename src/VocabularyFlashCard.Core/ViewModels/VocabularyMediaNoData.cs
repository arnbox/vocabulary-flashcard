using VocabularyFlashCard.Core.Models;

namespace VocabularyFlashCard.Core.ViewModels
{
	public class VocabularyMediaNoData
	{
		public int VocabularyMediaId { get; set; }
		public string MediaText { get; set; } = string.Empty;
		public MediaCategory MediaCategory { get; set; }
	}
}
