namespace VocabularyFlashCard.Core.ViewModels;

public class StatsViewModel
{
	public List<GroupCount> GroupCount { get; set; } = new List<GroupCount>();
	public int TotalVocab { get; set; } = new int();
	public int NewWords { get; set; } = new int();
	public VocabularyViewModel LongestWord { get; set; } = new VocabularyViewModel();
	public VocabularyViewModel LongestDefinition { get; set; } = new VocabularyViewModel();
	public VocabularyViewModel Oldest { get; set; } = new VocabularyViewModel();
}

public class GroupCount
{
	public byte Group { get; set; } = new byte();
	public int Count { get; set; } = new int();

	[JsonConverter(typeof(JsonDateFormatter))]
	public DateTime LastRead { get; set; }
}
