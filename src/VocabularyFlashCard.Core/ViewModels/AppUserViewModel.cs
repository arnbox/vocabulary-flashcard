namespace VocabularyFlashCard.Core.ViewModels;

public class AppUserViewModel
{
	public string Id { get; set; } = Guid.NewGuid().ToString();

	[Required(ErrorMessage = "Please, enter username")]
	[MaxLength(50, ErrorMessage = "Max length for {0} is {1} characters.")]
	public string UserName { get; set; } = string.Empty;


	[EmailAddress]
	[Required(ErrorMessage = "{0} is required")]
	[MaxLength(length: 50, ErrorMessage = "{0} can't be more than {1} characters")]
	public string Email { get; set; } = string.Empty;

	public string Password { get; set; } = string.Empty;
}
