using Microsoft.AspNetCore.StaticFiles;

namespace VocabularyFlashCard.Web.Services
{
	public static class WebApplicationExtensions
	{
		public static WebApplication AddSwagger(this WebApplication app)
		{
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
			return app;
		}

		public static WebApplication AddStaticFilesSettings(this WebApplication app)
		{
			// Set up custom content types - associating file extension to MIME type
			var provider = new FileExtensionContentTypeProvider();
			provider.Mappings[".avif"] = "image/avif";  // Add new mappings


			// static files cache
			const string oneYearInSeconds = "31536000";  // (60 * 60 * 24 * 365).ToString();
			app.UseStaticFiles(new StaticFileOptions
			{
				ContentTypeProvider = provider,
				OnPrepareResponse = ctx =>
				{
					ctx.Context.Response.Headers.Add("Cache-Control", $"public, max-age={oneYearInSeconds}");

				}
			});

			return app;
		}
	}
}
