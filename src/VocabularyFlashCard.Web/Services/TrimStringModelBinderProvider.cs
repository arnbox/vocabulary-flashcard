using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace VocabularyFlashCard.Web.Services
{
    public class TrimStringModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                Console.WriteLine($"ModelBinderProvider: Context is null");
            }

            Console.WriteLine($"===========================================================");
            Console.WriteLine($"ModelBinderProvider name: {context?.Metadata.Name}");
            Console.WriteLine($"ModelBinderProvider type: {context?.Metadata?.ModelType}");
            var modelType = context?.Metadata?.ModelType;
            if (modelType == typeof(string))
            {
                Console.WriteLine($"To string: {context?.ToString()}");
            }

            Console.WriteLine($"-----------------------------------------------------------");

            return null;
        }
    }
}
