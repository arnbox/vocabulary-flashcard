namespace VocabularyFlashCard.Core.ViewModels;

public class GroupListViewModel
{
	public List<VocabulariesGroup> GroupList { get; set; } = new List<VocabulariesGroup>();
}

public class VocabulariesGroup
{
	public byte GroupId { get; set; }
	public List<VocabularyItem> Vocabularies { get; set; } = new List<VocabularyItem>();

}
public class VocabularyItem
{
	public int Id { get; set; }
	public string Word { get; set; } = string.Empty;

	[JsonConverter(typeof(JsonDateFormatter))]
	public DateTime LastRead { get; set; }
}
