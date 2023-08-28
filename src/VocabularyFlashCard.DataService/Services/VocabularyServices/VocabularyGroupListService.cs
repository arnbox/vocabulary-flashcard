namespace VocabularyFlashCard.DataService.Services;
public class VocabularyGroupListService : IVocabularyGroupListService
{
	private readonly IVocabularyRepository _vocabularyRepository;

	public VocabularyGroupListService(IVocabularyRepository vocabularyRepository)
	{
		_vocabularyRepository = vocabularyRepository;
	}

	private class VocabulariesWithGroup : VocabularyItem
	{
		public byte GroupId { get; set; }
	}

	public async Task<GroupListViewModel> GetGroupList()
	{
		var vocabularies = await _vocabularyRepository.Vocabularies
			.Select(v => new VocabulariesWithGroup
			{
				Id = v.VocabularyId,
				LastRead = v.LastRead,
				Word = v.Word,
				GroupId = (v.NumRepeat == AppConstants.NoRepeat) ? (byte)0 : v.GroupId,
			})
			.OrderBy(v => v.GroupId)
			.ThenBy(v => v.LastRead)
			.ToListAsync();

		var groupList = buildGroupList(vocabularies);

		return groupList;
	}

	private static GroupListViewModel buildGroupList(List<VocabulariesWithGroup> vocabularies)
	{
		var groupList = new GroupListViewModel();
		var groups = vocabularies.Select(v => v.GroupId).Distinct();
		foreach (var group in groups)
		{
			var vocabulariesGroup = new VocabulariesGroup
			{
				GroupId = group,
			};

			var vocs = vocabularies.Where(v => v.GroupId == group).OrderBy(v => v.LastRead).ToList();
			vocabulariesGroup.Vocabularies.AddRange(vocs);

			groupList.GroupList.Add(vocabulariesGroup);
		}

		return groupList;
	}
}
