using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using VocabularyFlashCard.DataService.Services.VocabularyServices;

namespace VocabularyFlashCard.Web.Services;

public static class ServiceExtensions
{
	private static readonly string _defaultConnection = "DefaultConnection";

	public static IServiceCollection AddOptionReader(this IServiceCollection services, bool isProduction)
	{
		services
			.AddOptions<JwtOptions>()
			.BindConfiguration(JwtOptions.JwtSectionName)
			.ValidateDataAnnotations()
			.ValidateOnStart();

		if (isProduction)
		{
			// Decode values
			services.Configure<JwtOptions>(jwtOptions =>
			{
				jwtOptions.Issuer = jwtOptions.Issuer.Base64Decode();
				jwtOptions.Audience = jwtOptions.Audience.Base64Decode();
				jwtOptions.Key = jwtOptions.Key.Base64Decode();
			});
		}

		return services;
	}

	public static IServiceCollection AddIdentityConfig(this IServiceCollection services)
	{
		services.AddIdentity<AppUser, IdentityRole>
			(options => { options.Password = new CustomPasswordOptions(); })
			.AddEntityFrameworkStores<AppDbContext>()
			.AddDefaultTokenProviders();

		return services;
	}

	public static IServiceCollection AddUnicodeConfig(this IServiceCollection services)
	{
		// Add safe Unicode characterset to prevent being encoded in view render
		var unicodeRange = new[] { UnicodeRanges.BasicLatin, UnicodeRanges.Latin1Supplement, UnicodeRanges.Arabic };
		services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: unicodeRange));

		return services;
	}

	public static IServiceCollection AddDependencyGroup(this IServiceCollection services)
	{
		// Repositories
		services.AddScoped<IVocabularyRepository, EFVocabularyRepository>();
		services.AddScoped<IVocabularyMediaRepository, EFVocabularyMediaRepository>();

		// Services
		services.AddScoped<IVocabularyService, VocabularyService>();
		services.AddScoped<IVocabularyReminderService, VocabularyReminderService>();
		services.AddScoped<IVocabularyStatsService, VocabularyStatsService>();
		services.AddScoped<IVocabularyMediaService, VocabularyMediaService>();
		services.AddScoped<IVocabularyGroupListService, VocabularyGroupListService>();
		services.AddScoped<IAppUserService, AppUserService>();
		services.AddScoped<IAuthService, AuthService>();

		// Middlewares
		services.AddTransient<GlobalExceptionHandler>();

		return services;
	}

	public static IServiceCollection AddSqlConnection(this IServiceCollection services, IConfiguration config, bool isProduction)
	{
		var connectionString = config.GetConnectionString(_defaultConnection) ?? string.Empty;
		var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;

		if (isProduction)
		{
			connectionString = connectionString.Base64Decode();
		}

		services.AddDbContext<AppDbContext>(options =>
		{
			// display sql parameter and may include sensetive information and is a security risk
			//if (!isProduction)
			//{
			//	options.EnableSensitiveDataLogging(true);
			//}

			options
				.UseSqlServer(connectionString, b => b.MigrationsAssembly(assemblyName))
				.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
		});

		return services;
	}

	public static IServiceCollection AddJwtConfig(this IServiceCollection services)
	{
		var jwtOptions = getJwtOption(services);

		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters()
				{
					ValidateActor = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = jwtOptions.Issuer,
					ValidAudience = jwtOptions.Audience,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))
				};
			}
			);

		return services;
	}

	public static IServiceCollection AddMemoryEncryption(this IServiceCollection services)
	{
		services
			.AddDataProtection()
			.UseCryptographicAlgorithms(
				new AuthenticatedEncryptorConfiguration
				{
					EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
					ValidationAlgorithm = ValidationAlgorithm.HMACSHA512
				}
			);
		return services;
	}

	public static string AddCorsConfig(this IServiceCollection services)
	{
		const string corsPolicyName = "CorsPolicy";
		var jwtOptions = getJwtOption(services);

		services.AddCors(options => options.AddPolicy(corsPolicyName, builder =>
							builder
							.AllowAnyMethod()
							.AllowAnyHeader()
							.AllowCredentials()
							.SetPreflightMaxAge(TimeSpan.FromMinutes(jwtOptions.ExpiryMinutes))
							.WithOrigins(jwtOptions.Audience))); // front-end

		return corsPolicyName;
	}

	public static IServiceCollection AddHttpCompression(this IServiceCollection services)
	{
		services.AddResponseCompression(options =>
		{
			options.EnableForHttps = true;
			options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
			{
				"image/svg+xml",
				"application/atom+xml"
			});
		});

		services.Configure<BrotliCompressionProviderOptions>(options =>
		{
			options.Level = CompressionLevel.SmallestSize;
		});

		services.Configure<GzipCompressionProviderOptions>(options =>
		{
			options.Level = CompressionLevel.SmallestSize;
		});

		return services;
	}

	private static JwtOptions getJwtOption(IServiceCollection services)
	{
		var serviceProvider = services.BuildServiceProvider();
		var jwtOptions = serviceProvider.GetRequiredService<IOptions<JwtOptions>>();

		return jwtOptions.Value;
	}
}
