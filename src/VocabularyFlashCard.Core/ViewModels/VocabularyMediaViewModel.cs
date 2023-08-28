namespace VocabularyFlashCard.Core.ViewModels;

public class VocabularyMediaViewModel
{
	public int VocabularyMediaId { get; set; }

	[StringLength(250), Display(Name = "Sample Text")]
	public string MediaText { get; set; } = string.Empty;

	public MediaCategory MediaCategory { get; set; }

	public string FileName { get; set; } = string.Empty;

	public string FileData { get; set; } = string.Empty;
	//public byte[] FileData { get; set; } = Array.Empty<byte>();
}