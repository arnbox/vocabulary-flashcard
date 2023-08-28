namespace VocabularyFlashCard.Web.Middlewares;

public class GlobalExceptionHandler : IMiddleware
{
	private readonly ILogger<GlobalExceptionHandler> _logger;

	private readonly JsonSerializerOptions _jsonOptions;

	public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
	{
		_logger = logger;

		_jsonOptions = new JsonSerializerOptions { WriteIndented = true };
	}

	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		try
		{
			await next(context);
		}
		catch (Exception e)
		{
			var logId = Guid.NewGuid();

			_logger.LogError(e, "Error Id: {logId}, {e.Message}", logId, e.Message);

			var problemDetails = new ProblemDetails
			{
				Status = StatusCodes.Status500InternalServerError,
				Type = "https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/500",
				Title = "Internal server error",
				Detail = $"An internal server error has occured. Error Id: {logId}",
			};

			context.Response.StatusCode = StatusCodes.Status500InternalServerError;
			await context.Response.WriteAsJsonAsync(problemDetails, _jsonOptions);
		}
	}
}
