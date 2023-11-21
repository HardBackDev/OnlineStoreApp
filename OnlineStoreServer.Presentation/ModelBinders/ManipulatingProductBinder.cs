using Entities.Exceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace OnlineStoreServer.Presentation.ModelBinders
{
    public class ManipulatingProductBinder : IModelBinder
    {
        static Dictionary<string, Type> ManipulatingTypesCache = new();
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var request = bindingContext.HttpContext.Request;
            var categoryName = bindingContext.ValueProvider.GetValue("category").FirstValue;

            Type manipulatingDtoType;
            if (!ManipulatingTypesCache.TryGetValue(categoryName, out manipulatingDtoType))
            {
                manipulatingDtoType = Type.GetType($"Shared.Dto.ProductCategoriesDtos.{categoryName}Dtos.{categoryName}ForManipulating, Shared", false) ??
                    throw new BadRequestException($"The searched type for manupulating {categoryName} not found. " +
                    $"searched for the path \"Shared.Dto.ProductCategoriesDtos.{categoryName}Dtos.{categoryName}ForManipulating\"");

                ManipulatingTypesCache.Add(categoryName, manipulatingDtoType);
            }

            using var reader = new StreamReader(request.Body, Encoding.UTF8);
            var body = await reader.ReadToEndAsync();
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            JObject? jsonObject = (JObject)JsonConvert.DeserializeObject(body, settings);
            var product = jsonObject?.ToObject(manipulatingDtoType);

            bindingContext.Model = product;
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
        }
    }
}
