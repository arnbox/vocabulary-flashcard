using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace VocabularyFlashCard.DataService.Services.Interfaces;

public interface IAppUserService
{
	Task<List<AppUserViewModel>> Users();
	Task<AppUserViewModel> UserById(string id);
	Task<AppUserViewModel> SaveAsync(AppUserViewModel appUserViewModel);
	Task CheckCustomValidationAsync(AppUserViewModel appUserViewModel, ModelStateDictionary modelState);
}
