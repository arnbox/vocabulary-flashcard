using Microsoft.AspNetCore.Identity;

namespace VocabularyFlashCard.Core.Internal
{
	public class CustomPasswordOptions : PasswordOptions
	{
		public new int RequiredLength { get; set; } = 5;
		public new bool RequireNonAlphanumeric { get; set; } = false;
		public new bool RequireLowercase { get; set; } = false;
		public new bool RequireUppercase { get; set; } = false;
		public new bool RequireDigit { get; set; } = false;
	}
}
