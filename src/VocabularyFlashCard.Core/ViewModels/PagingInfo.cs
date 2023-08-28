namespace VocabularyFlashCard.Core.ViewModels;

public class PagingInfo
{
	public int TotalItems { get; set; }

	[Range(0, 10_000)]
	public int ItemsPerPage { get; set; } = 100;

	[Range(1, 10_000)]
	public int CurrentPage { get; set; } = 1;

	[StringLength(1024)]
	public string SearchPhrase { get; set; } = string.Empty;

	public VocabularyListType ListType { get; set; } = VocabularyListType.Default;
	public int TotalPages =>
		(int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
}
