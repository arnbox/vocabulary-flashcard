namespace VocabularyFlashCard.Core.Internal;

public class JwtOptions
{
	// Name of options group in appsettings file
	public const string JwtSectionName = "Jwt";

	[Required]
	[StringLength(512, MinimumLength = 64)]
	public string Key { get; set; } = string.Empty;

	[Required]
	//[Url]
	public string Issuer { get; set; } = string.Empty;

	[Required]
	//[Url]
	public string Audience { get; set; } = string.Empty;

	[Required]
	[Range(0, int.MaxValue)]
	public int ExpiryMinutes { get; set; } = 60;
}
