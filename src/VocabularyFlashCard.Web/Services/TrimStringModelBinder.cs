using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace VocabularyFlashCard.Web.Services
{
	public class TrimStringModelBinder : IModelBinder
	{
		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			if (bindingContext.ModelType == typeof(string))
			{
				Console.WriteLine($"Name: {bindingContext.ModelName}");
				Console.WriteLine($"Value: {bindingContext.ValueProvider.GetValue(bindingContext.ModelName)}");
			}
			else
			{
				Console.WriteLine($"Model type: {bindingContext.ModelType}");
			}

			return Task.CompletedTask;
		}

		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			Console.WriteLine($"BindModel, Model type: {bindingContext.ModelType}");

			return "";
		}

	}

}
