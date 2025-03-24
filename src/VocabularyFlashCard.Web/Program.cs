var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
	var builder = WebApplication.CreateBuilder(args);

	builder.Logging.ClearProviders();
	builder.Host.UseNLog();

	var isProduction = builder.Environment.IsProduction();
	var services = builder.Services;
	var config = builder.Configuration;

	services
		.AddMemoryEncryption()
		.AddSqlConnection(config, isProduction)
		.AddOptionReader(isProduction)
		.AddJwtConfig()
		.AddIdentityConfig()
		.AddUnicodeConfig()
		.AddHttpCompression()
		.AddDependencyGroup(builder);


	var corsPolicy = services.AddCorsConfig();

	//services.AddControllers(option => option.ModelBinderProviders.Insert(0, new TrimStringModelBinderProvider()));
	services.AddControllers();
	services.AddEndpointsApiExplorer();
	services.AddSwaggerGen();

	services.AddHealthChecks()
			.AddDbContextCheck<AppDbContext>();

	services.AddAuthorization();

	var app = builder.Build();

	app.AddSwagger();

	app.UseRouting();
	app.UseCors(corsPolicy);

	app.UseResponseCompression();
	app.AddStaticFilesSettings();

	app.UseAuthentication();
	app.UseAuthorization();

	app.UseMiddleware<GlobalExceptionHandler>();

	app.MapControllers();
	app.MapHealthChecks(AppConstants.HealthCheck);
	app.MapFallbackToFile(AppConstants.HomePage);

	app.Run();
}
catch (Exception exception)
{
	logger.Error(exception, "Stopped program because of exception");
	throw;
}
finally
{
	LogManager.Shutdown();
}
