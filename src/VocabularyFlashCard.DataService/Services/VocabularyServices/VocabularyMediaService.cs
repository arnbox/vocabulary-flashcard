using WorkTimeReport.Infrastructure.Extensions;

namespace VocabularyFlashCard.DataService.Services;

public class VocabularyMediaService : IVocabularyMediaService
{
	private readonly IVocabularyMediaRepository _vocabularyMediaRepository;
	private const int _newVocabularyMediaIdStart = 1_000_000_000;

	public VocabularyMediaService(IVocabularyMediaRepository vocabularyMediaRepository)
	{
		_vocabularyMediaRepository = vocabularyMediaRepository;
	}

	public async Task<List<VocabularyMedia>> VocabularyMediaList(int vocabularyId)
	{
		var vm = await _vocabularyMediaRepository
		.VocabularyMedia
		.Where(m => m.VocabularyId == vocabularyId)
		.ToListAsync();

		vm ??= new List<VocabularyMedia>();

		return vm;
	}

	public async Task<byte[]> VocabularyMediumFileDataAsync(int id)
	{
		var vm = await _vocabularyMediaRepository
			.VocabularyMedia
			.Where(m => m.VocabularyMediaId == id)
			.Select(m => m.FileData)
			.FirstOrDefaultAsync();

		vm ??= Array.Empty<byte>();

		return vm;
	}


	public async Task SaveMediaAsync(List<VocabularyMedia> receivedMedia, int receivedVocabularyId)
	{
		if (receivedVocabularyId != default)
		{
			var receivedMediaWithoutNew = receivedMedia
				?.Where(m => m.VocabularyMediaId < _newVocabularyMediaIdStart)
				?.ToList();

			var currentMedia = await VocabularyMediaList(receivedVocabularyId);

			if (currentMedia is not null && receivedMediaWithoutNew is not null)
			{
				// DELETE REMOVED MEDIA
				await deleteMediaAsync(receivedMediaWithoutNew, currentMedia);

				// UPDATE MEDIA
				await updateMediaAsync(receivedMediaWithoutNew, currentMedia);
			}
		}

		// ADD NEW
		var receivedNewMedia = receivedMedia
			?.Where(m =>
						m.VocabularyMediaId >= _newVocabularyMediaIdStart &&
						m.FileData.Any())
			?.ToList();

		if (receivedNewMedia.HasValue())
		{
			await addMediaAsync(receivedNewMedia);
		}
	}

	private async Task addMediaAsync(List<VocabularyMedia> receivedNewMedia)
	{
		foreach (var medium in receivedNewMedia)
		{
			await _vocabularyMediaRepository.AddAsync(medium);
		}
	}

	private async Task deleteMediaAsync(List<VocabularyMedia> receivedMedia, List<VocabularyMedia> currentMedia)
	{
		foreach (var medium in currentMedia)
		{
			// Delete current medium that are not in received media
			if (!receivedMedia.Any(m => m.VocabularyMediaId == medium.VocabularyMediaId))
			{
				await _vocabularyMediaRepository.DeleteAsync(medium);
			}
		}
	}


	private async Task updateMediaAsync(List<VocabularyMedia> receivedMedia, List<VocabularyMedia> currentMedia)
	{
		foreach (var receivedMedium in receivedMedia)
		{
			var currentMedium = currentMedia
				?.FirstOrDefault(m => m.VocabularyMediaId == receivedMedium.VocabularyMediaId);

			if (currentMedium is not null)
			{
				// if has changed
				if (receivedMedium.MediaText != currentMedium.MediaText ||
					receivedMedium.MediaCategory != currentMedium.MediaCategory ||
					receivedMedium.FileData.Length > 1)
				{
					// if has no new file, get file from current
					if (receivedMedium.FileData.Length == 0)
					{
						receivedMedium.FileData = currentMedium.FileData;
						receivedMedium.FileName = currentMedium.FileName;
					}

					// Update
					await _vocabularyMediaRepository.UpdateAsync(receivedMedium);
				}

			}
		}
	}

}
