namespace VocabularyFlashCard.Core.Models;

[Table("VocabularyMedia")]
public class VocabularyMedia
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public int VocabularyMediaId { get; set; }

	public int VocabularyId { get; set; }

	[StringLength(250), Display(Name = "Sample Text")]
	public string MediaText { get; set; } = string.Empty;

	public MediaCategory MediaCategory { get; set; }

	[Required, StringLength(255), Display(Name = "File Name")]

	public string FileName { get; set; } = string.Empty;

	public byte[] FileData { get; set; } = Array.Empty<byte>();


	// Navigation property
	public Vocabulary? Vocabulary { get; set; } = null;
}
