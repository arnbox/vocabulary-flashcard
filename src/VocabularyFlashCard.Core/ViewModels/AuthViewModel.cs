namespace VocabularyFlashCard.Core.ViewModels;

public class AuthViewModel
{
	public bool IsLoggedIn { get; set; } = false;
	public string Email { get; set; } = string.Empty;
	public string Id { get; set; } = string.Empty;
	public string Bearer { get; set; } = string.Empty;
}
