namespace VocabularyFlashCard.DataService.Services;
public class VocabularyService : IVocabularyService
{
    private readonly IVocabularyRepository _vocabularyRepository;
    private readonly IVocabularyMediaService _vocabularyMediaService;

    public VocabularyService(
        IVocabularyRepository vocabularyRepository,
        IVocabularyMediaService vocabularyMediaService)
    {
        _vocabularyRepository = vocabularyRepository;
        _vocabularyMediaService = vocabularyMediaService;
    }

    public async Task<VocabularyListViewModel> VocabulariesAsync(string? searchPhrase, int? page, int? itemsPerPage)
    {
        var pagingInfo = VocabularyServiceBuilder.BuildPagingInfo(searchPhrase, page, itemsPerPage);

        // Count all
        var vocQuery = _vocabularyRepository.Vocabularies;
        var queryAll = VocabularyServiceBuilder
            .BuildQuery(vocQuery, pagingInfo, getAll: true);
        pagingInfo.TotalItems = await queryAll.CountAsync();

        var query = VocabularyServiceBuilder
            .BuildQuery(vocQuery, pagingInfo);

        var vocList = new VocabularyListViewModel()
        {
            PagingInfo = pagingInfo,
            Vocabularies = await VocabularyServiceBuilder.GetVocabs(query)
        };

        return vocList;
    }

    public async Task<DifficultVocabulariesViewModel> DifficultMarkedVocabularies()
    {
        var vocQuery = _vocabularyRepository.Vocabularies;

        var pagingInfoDifficult = new PagingInfo() { ListType = VocabularyListType.Difficult };
        var queryDifficult = VocabularyServiceBuilder.BuildQuery(vocQuery, pagingInfoDifficult);

        var pagingInfoMarked = new PagingInfo() { ListType = VocabularyListType.Marked };
        var queryMarked = VocabularyServiceBuilder.BuildQuery(vocQuery, pagingInfoMarked);

        var pagingInfoLastRead = new PagingInfo() { ListType = VocabularyListType.LastRead };
        var queryLastRead = VocabularyServiceBuilder.BuildQuery(vocQuery, pagingInfoLastRead);


        var list = new DifficultVocabulariesViewModel()
        {
            DifficultVocabularies = await VocabularyServiceBuilder.GetVocabs(queryDifficult),
            MarkedVocabularies = await VocabularyServiceBuilder.GetVocabs(queryMarked),
            LastReadVocabularies = await VocabularyServiceBuilder.GetVocabs(queryLastRead),
        };

        return list;
    }


    public async Task<string[]> GetAllVocabulariesWord()
    {
        var vocWords = await _vocabularyRepository.Vocabularies
                .Select(v => v.Word)
                .ToArrayAsync();

        return vocWords;
    }

    public async Task<VocabularyMarkedViewModel> ToggleVocabularyMarkAsync(int id)
    {
        var marked = await _vocabularyRepository
            .Vocabularies
            .Where(v => v.VocabularyId == id)
            .Select(v => v.Marked ?? false)
            .FirstOrDefaultAsync();

        var voc = new Vocabulary()
        {
            VocabularyId = id,
            Marked = !marked
        };

        var db = _vocabularyRepository.GetContext();
        db.Attach<Vocabulary>(voc).Property(v => v.Marked).IsModified = true;
        var affectRows = await db.SaveChangesAsync();

        var vocabularyMarkedViewModel = new VocabularyMarkedViewModel();

        if (affectRows > 0)
        {
            vocabularyMarkedViewModel.VocabularyId = voc.VocabularyId;
            vocabularyMarkedViewModel.Marked = (bool)voc.Marked;
        }

        return vocabularyMarkedViewModel;
    }

    public async Task SaveAsync(VocabularyWithMediaViewModel vocabularyWithMedia)
    {
        // Vocabulary
        var vocabulary = VocabularyServiceBuilder.BuildVocabulary(vocabularyWithMedia); // Build
        var VocabularyId = await _vocabularyRepository.SaveAsync(vocabulary);     // Save

        // Media
        var receivedMedia = VocabularyServiceBuilder.BuildVocabularyMedia(vocabularyWithMedia, VocabularyId);
        await _vocabularyMediaService.SaveMediaAsync(receivedMedia, vocabularyWithMedia.VocabularyId);
    }
}
