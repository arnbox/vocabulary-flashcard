namespace VocabularyFlashCard.Core.Internal;

public class JwtOptions
{
	/// <summary>
	/// Name of the options group in the appsettings file.
	/// </summary>
	public const string JwtSectionName = "Jwt";

	/// <summary>
	/// The secret key used for signing the JWT.
	/// </summary>
	[Required]
	[StringLength(512, MinimumLength = 64)]
	public string Key { get; set; } = string.Empty;

	/// <summary>
	/// The issuer of the JWT.
	/// </summary>
	[Required]
	//[Url]
	public string Issuer { get; set; } = string.Empty;

	/// <summary>
	/// The audience for the JWT.
	/// </summary>
	[Required]
	//[Url]
	public string Audience { get; set; } = string.Empty;

	/// <summary>
	/// The expiry time of the JWT in minutes.
	/// </summary>
	[Required]
	[Range(0, int.MaxValue)]
	public int ExpiryMinutes { get; set; } = (60 * 24) * 7;
}
