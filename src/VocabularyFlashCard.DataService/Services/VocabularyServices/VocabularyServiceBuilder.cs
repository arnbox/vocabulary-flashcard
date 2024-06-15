using System.Linq.Expressions;
using WorkTimeReport.Infrastructure.Extensions;

namespace VocabularyFlashCard.DataService.Services;

internal static class VocabularyServiceBuilder
{
	internal static IQueryable<Vocabulary> BuildQuery(IQueryable<Vocabulary> query, PagingInfo pagingInfo, bool getAll = false)
	{
		switch (pagingInfo.ListType)
		{
			case VocabularyListType.Search:
				var searchPhrase = pagingInfo.SearchPhrase.ToLower();

				query = query
					.Where(v =>
						v.Word.ToLower() == searchPhrase ||
						v.Word.ToLower().Contains(searchPhrase) ||
						v.Definition.ToLower().Contains(searchPhrase) ||
						v.VocabularyMedias!.Any(m => m.MediaText.ToLower().Contains(searchPhrase))
					)
					.OrderBy(v => v.Word.ToLower() == searchPhrase ? 0 : 1)
					.ThenBy(v => v.Word.ToLower().Contains(searchPhrase) ? 0 : 1)
					.ThenBy(v => v.VocabularyId);
				break;

			case VocabularyListType.Marked:
				query = query.Where(v => v.Marked ?? false)
					.OrderByDescending(v => v.GroupId)
					.ThenByDescending(v => v.LastRead);
				break;

			case VocabularyListType.Difficult:
				query = query
					.Where(v => v.GroupId != AppConstants.MaxGroup)
					.OrderByDescending(v => (v.NumRepeat - v.GroupId))
					.ThenByDescending(v => v.GroupId);
				break;

			case VocabularyListType.LastRead:
				query = query
					.Where(v => v.NumRepeat != AppConstants.NoRepeat)
					.OrderByDescending(v => v.LastRead);
				break;

			case VocabularyListType.Default:
			default:
				if (!getAll)
				{
					query = query.OrderByDescending(v => v.VocabularyId);
				}
				break;
		}

		if (!getAll)
		{
			query = query
				.Skip((pagingInfo.CurrentPage - 1) * pagingInfo.ItemsPerPage)
				.Take(pagingInfo.ItemsPerPage)
				.Include(m => m.VocabularyMedias);
		}

		return query;
	}

	internal static async Task<List<VocabularyViewModel>> GetVocabs(IQueryable<Vocabulary> query)
	{
		var vocabularyList = await query
			.Select(VocabularyViewModelSelector)
			.ToListAsync();

		return vocabularyList;
	}

	internal static Expression<Func<Vocabulary, VocabularyViewModel>>
		VocabularyViewModelSelector => (v => new VocabularyViewModel
		{
			VocabularyId = v.VocabularyId,
			Word = v.Word,
			Definition = v.Definition,
			GroupId = v.GroupId,
			NumRepeat = v.NumRepeat,
			LastRead = v.LastRead,
			Marked = v.Marked ?? false,
			VocabularyMedia =
			v.VocabularyMedias!.Select(m => new VocabularyMediaNoData
			{
				VocabularyMediaId = m.VocabularyMediaId,
				MediaCategory = m.MediaCategory,
				MediaText = m.MediaText
			})
		});

	internal static Vocabulary BuildVocabulary(VocabularyWithMediaViewModel vocabularyWithMedia)
	{
		var isNew = (vocabularyWithMedia.VocabularyId == default);

		var vocabulary = new Vocabulary()
		{
			VocabularyId = vocabularyWithMedia.VocabularyId,
			Word = vocabularyWithMedia.Word,
			Definition = vocabularyWithMedia.Definition,
			Marked = vocabularyWithMedia.Marked,
			GroupId = isNew ? AppConstants.FirstGroup : vocabularyWithMedia.GroupId,
			LastRead = isNew ? DateTime.Now.ToCustomeTimeZone() : vocabularyWithMedia.LastRead,
			NumRepeat = isNew ? AppConstants.NoRepeat : vocabularyWithMedia.NumRepeat,
		};

		return vocabulary;
	}

	internal static List<VocabularyMedia> BuildVocabularyMedia(VocabularyWithMediaViewModel vocabularyWithMedia, int vocabularyId)
	{
		var VocabularyMediaList = new List<VocabularyMedia>();

		if (vocabularyWithMedia?.VocabularyMedia is not null)
		{
			foreach (var mediumViewModel in vocabularyWithMedia.VocabularyMedia)
			{
				var vocabularyMedia = new VocabularyMedia()
				{
					VocabularyMediaId = mediumViewModel.VocabularyMediaId,
					VocabularyId = vocabularyId,
					Vocabulary = null,
					MediaText = mediumViewModel.MediaText,
					MediaCategory = mediumViewModel.MediaCategory,
					FileName = mediumViewModel.FileName,
					FileData = Convert.FromBase64String(mediumViewModel.FileData)
				};

				VocabularyMediaList.Add(vocabularyMedia);
			}
		}
		return VocabularyMediaList;
	}

	internal static PagingInfo BuildPagingInfo(string? searchPhrase, int? page, int? itemsPerPage)
	{
		return new PagingInfo()
		{
			ItemsPerPage = itemsPerPage ?? AppConstants.ItemsPerPage,
			SearchPhrase = searchPhrase ?? string.Empty,
			CurrentPage = page ?? AppConstants.FirstPage,
			ListType = string.IsNullOrWhiteSpace(searchPhrase) ? VocabularyListType.Default : VocabularyListType.Search
		};
	}
}
