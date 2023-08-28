namespace VocabularyFlashCard.Core.ViewModels;

public class VocabularyViewModel
{
	public int VocabularyId { get; set; }
	public string Word { get; set; } = string.Empty;
	public string Definition { get; set; } = string.Empty;
	public short NumRepeat { get; set; }
	public byte GroupId { get; set; }

	[JsonConverter(typeof(JsonDateFormatter))]
	public DateTime LastRead { get; set; }
	public bool Marked { get; set; }
	//		public string VocabularyMarkedJson { get; set; }
	public IEnumerable<VocabularyMediaNoData>? VocabularyMedia { get; set; } = Enumerable.Empty<VocabularyMediaNoData>();
}


public class VocabularyWithMediaViewModel
{
	public int VocabularyId { get; set; }
	public string Word { get; set; } = string.Empty;
	public string Definition { get; set; } = string.Empty;
	public short NumRepeat { get; set; }
	public byte GroupId { get; set; }

	[JsonConverter(typeof(JsonDateFormatter))]
	public DateTime LastRead { get; set; }
	public bool Marked { get; set; } = false;
	//		public string VocabularyMarkedJson { get; set; }
	public IEnumerable<VocabularyMediaViewModel>? VocabularyMedia { get; set; } = Enumerable.Empty<VocabularyMediaViewModel>();
}
