using WorkTimeReport.Infrastructure.Extensions;

namespace VocabularyFlashCard.DataService.Services;
public class VocabularyReminderService : IVocabularyReminderService
{
	private readonly IVocabularyRepository _vocabularyRepository;

	public VocabularyReminderService(IVocabularyRepository vocabularyRepository)
	{
		_vocabularyRepository = vocabularyRepository;
	}

	public async Task<VocabularyViewModel> NextVocabulary(VocabularyReminderViewModel vocabularyReminderViewModel)
	{
		await updateVocabularyReply(vocabularyReminderViewModel);

		var voc = await getNextVocabulary(vocabularyReminderViewModel);
		return voc;
	}

	private async Task updateVocabularyReply(VocabularyReminderViewModel reply)
	{
		if (reply.VocabularyId == default)
		{
			return;
		}

		var voc = await _vocabularyRepository.Vocabularies
			.FirstOrDefaultAsync(v => v.VocabularyId == reply.VocabularyId);

		if (voc != default)
		{
			voc.GroupId = reply.KnowIt ? Convert.ToByte(Math.Min(voc.GroupId + 1, AppConstants.MaxGroup)) : AppConstants.FirstGroup;
			voc.LastRead = DateTime.Now.ToCustomeTimeZone();

			// Skip repeat count for last group
			if (voc.GroupId != AppConstants.MaxGroup)
			{
				voc.NumRepeat = Convert.ToInt16(Math.Min(voc.NumRepeat + 1, short.MaxValue - 1));
			}

			await _vocabularyRepository.SaveAsync(voc);
		}
	}

	private async Task<VocabularyViewModel> getNextVocabulary(VocabularyReminderViewModel vocabulary)
	{
		var groupId = vocabulary.NextVocabularyGroupId;
		VocabularyViewModel? voc = null;
		do
		{
			var query = _vocabularyRepository.Vocabularies.Where(v => v.GroupId == groupId);

			if (groupId == AppConstants.FirstGroup)
			{
				// If there is new vocabulary show them first
				var newVocabs = query.Where(x => x.NumRepeat == AppConstants.NoRepeat);
				if (newVocabs.Any())
				{
					query = newVocabs;
				}
			}

			voc = await query
						.Select(VocabularyServiceBuilder.VocabularyViewModelSelector)
						.OrderBy(v => v.LastRead)
						.FirstOrDefaultAsync();

			// prepare groupId for next run if no vacabulary found in this group
			groupId = (groupId == AppConstants.MaxGroup) ? AppConstants.FirstGroup : groupId + 1;

		} while (voc == default);

		return voc;
	}
}
