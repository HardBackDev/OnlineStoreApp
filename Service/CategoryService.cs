using Contracts.RepositoryContracts;
using Contracts.ServiceContracts;
using Entities.Exceptions;
using Entities.Models;
using Shared.Dto.CategoryDtos;
using Shared.RequestFeatures;
using Shared.RequestFeatures.DtoAttributes;
using Shared.RequestFeatures.ParametersAttributes;
using Shared.RequestFeatures.ParametersModels;
using System.Dynamic;
using System.Reflection;

namespace Service
{
    public class CategoryService : ICategoryService
    {
        readonly IRepositoryManager _repo;
        public CategoryService(IRepositoryManager repo)
        {
            _repo = repo;
        }

        static Dictionary<string, ParametersMetaData> ParametersMetadatasCash = new();
        static Dictionary<string, ManipulatingDtoMetadata> ManipulatingsMetadatasCash = new();

        public async Task<IEnumerable<CategoryDto>> GetCategories()
        {
            return await _repo.CategoryRepo.GetCategories();
        }

        public ParametersMetaData GetCategoryParametersMetadata(string category)
        {
            var categoryParametersName = category + "Parameters";

            if (ParametersMetadatasCash.TryGetValue(categoryParametersName, out var metadata))
                return metadata;

            Type categoryParametersType = Type.GetType($"Shared.RequestFeatures.ProductsParameters.{categoryParametersName}, Shared", false) ??
                throw new BadRequestException($"The searched category {categoryParametersName} not found. " +
                $"searched for the path \"Shared.RequestFeatures.EntitiesParameters.{categoryParametersName}\"");

            var parametersObject = GetParametersObject(categoryParametersType);
            ParametersMetadatasCash.Add(categoryParametersName, parametersObject);
            return parametersObject;
        }

        private ParametersMetaData GetParametersObject(Type parametersType)
        {
            var parametersMetaData = new ParametersMetaData();
            parametersMetaData.OrderByColumns.Add("Price");
            parametersMetaData.ParametersNames.Add("MinPrice", "Min Price");
            parametersMetaData.ParametersNames.Add("MaxPrice", "Max Price");
            parametersMetaData.Parameters.TryAdd("MinPrice", "number");
            parametersMetaData.Parameters.TryAdd("MaxPrice", "number");

            var orderByColumnsAttribute = Attribute.GetCustomAttribute(parametersType, typeof(OrderByColumnsAttribute)) as OrderByColumnsAttribute;
            if (orderByColumnsAttribute != null)
                parametersMetaData.OrderByColumns.AddRange(orderByColumnsAttribute.Columns);

            foreach (var prop in parametersType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                if (prop.IsDefined(typeof(SearchValuesAttribute)))
                {
                    var attributes = Attribute.GetCustomAttributes(prop, typeof(SearchValuesAttribute)) as SearchValuesAttribute[];
                    foreach(var searchValuesAttribute in attributes)
                    {
                        if (searchValuesAttribute.DependentOnParameter == null)
                        {
                            parametersMetaData.ParametersSearchValues.Add(prop.Name, searchValuesAttribute.SearchingValues);
                        }
                        else
                        {
                            var depndentSearchValues = new DependentSearchParameter()
                            {
                                DependentOnParameter = searchValuesAttribute.DependentOnParameter,
                                DependentOnValues = searchValuesAttribute.DependentOnValues,
                                Parameter = prop.Name,
                                SearchValues = searchValuesAttribute.SearchingValues
                            };
                            parametersMetaData.DependentSearchValues.Add(depndentSearchValues);
                        }
                    }
                }
                if (prop.IsDefined(typeof(ParameterNameAttribute)))
                {
                    var parameterNameAttribute = Attribute.GetCustomAttribute(prop, typeof(ParameterNameAttribute)) as ParameterNameAttribute;
                    parametersMetaData.ParametersNames.Add(prop.Name, parameterNameAttribute.ParameterName);
                }

                Type propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                AddPropertyToObject(parametersMetaData.Parameters, propType, prop.Name);
            }

            return parametersMetaData;
        }

        public ManipulatingDtoMetadata GetManipulatingDtoMetadata(string category)
        {
            if (ManipulatingsMetadatasCash.TryGetValue(category, out var objectForManipulating))
                return objectForManipulating;

            Type manipulatingDtoType = Type.GetType($"Shared.Dto.ProductCategoriesDtos.{category}Dtos.{category}ForManipulating, Shared", false) ??
                throw new BadRequestException($"The searched type for manupulating {category} not found. " +
                $"searched for the path \"Shared.Dto.ProductCategoriesDtos.{category}Dtos.{category}ForManipulating\"");

            var dtoObject = GetObjectForManupulatingDto(manipulatingDtoType);
            ManipulatingsMetadatasCash.Add(category, dtoObject);
            return dtoObject;
        }

        private ManipulatingDtoMetadata GetObjectForManupulatingDto(Type type)
        {
            var manipulatingDtoMetadata = new ManipulatingDtoMetadata();
            Type valuesType = typeof(DefaultValuesAttribute);
            DefaultValuesAttribute valuesAttribute;

            foreach (var prop in type.GetProperties())
            {
                Type propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                if (prop.IsDefined(typeof(DefaultValuesAttribute)))
                {
                    valuesAttribute = Attribute.GetCustomAttribute(prop, valuesType) as DefaultValuesAttribute;
                    if (valuesAttribute.DefaultValues != null)
                    {
                        manipulatingDtoMetadata.PropertiesValues.Add(prop.Name, valuesAttribute.DefaultValues);
                        if(valuesAttribute.OnlyDefaultValues)
                            manipulatingDtoMetadata.OnlyDefaultValuesProperties.Add(prop.Name);
                    }

                }

                AddPropertyToObject(manipulatingDtoMetadata.ManipulatinObject, propType, prop.Name);
            }
            return manipulatingDtoMetadata;
        }

        static Type[] numberTypes =
        {
            typeof(int),
            typeof(float),
            typeof(double),
            typeof(long)
        };

        private void AddPropertyToObject(ExpandoObject expandoObject, Type type, string propName)
        {
            if (numberTypes.Contains(type))
                expandoObject.TryAdd(propName, "number");
            else if (type == typeof(string))
                expandoObject.TryAdd(propName, "text");
            else if (type == typeof(DateOnly))
                expandoObject.TryAdd(propName, "date");
        }

        private async Task CheckCategoryExist(string category)
        {
            if (!category.Equals("Products", StringComparison.OrdinalIgnoreCase) && await _repo.CategoryRepo.GetCategoryByName(category) == null)
                throw new NotFoundException(nameof(Category), category);
        }
    }
}
