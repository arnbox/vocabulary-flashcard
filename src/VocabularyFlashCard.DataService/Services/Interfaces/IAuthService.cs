namespace VocabularyFlashCard.DataService.Services.Interfaces;

public interface IAuthService
{
	Task<AuthViewModel> Login(LoginViewModel loginViewModel);
	Task Logout();
}
