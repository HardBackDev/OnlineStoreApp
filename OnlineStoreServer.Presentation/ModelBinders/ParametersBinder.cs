using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shared.RequestFeatures.ProductsParameters;
using Shared.utilities;
using System.Linq.Expressions;
using System.Reflection;

namespace OnlineStoreServer.Presentation.ModelBinders
{
    public class ParametersBinder : IModelBinder
    {
        static Dictionary<string, Type> parametersTypeCash = new Dictionary<string, Type>();
        static Dictionary<Type, Func<IValueProvider, ProductsParameters>> bindParametersFuncsCash = new();

        static MethodInfo
            getValueMethod = typeof(IValueProvider).GetMethod("GetValue"),
            intParseMethod = typeof(int).GetMethod("Parse", new[] { typeof(string) }),
            doubleParseMethod = typeof(double).GetMethod("Parse", new[] { typeof(string) }),
            floatParseMethod = typeof(float).GetMethod("Parse", new[] { typeof(string) }),
            longParseMethod = typeof(long).GetMethod("Parse", new[] { typeof(long) }),
            decimalParseMethod = typeof(decimal).GetMethod("Parse", new[] { typeof(long) }),
            dateOnlyParseMethod = typeof(DateOnly).GetMethod("Parse", new[] { typeof(string) }),
            dateTimeParceMethod = typeof(DateTime).GetMethod("Parse", new[] { typeof(string) });

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            IValueProvider valueProvider;
            var request = bindingContext.HttpContext.Request;
            if (!request.QueryString.HasValue)
            {
                bindingContext.Result = ModelBindingResult.Success(new ProductsParameters());
                return Task.CompletedTask;
            }
            string searchedCategory = bindingContext.ValueProvider.GetValue("category").FirstValue;
            var parametersType = GetParametersType(searchedCategory);
            valueProvider = bindingContext.ValueProvider;
            var objectBinderLambda = BuildParametersBinderLambda(parametersType);
            

            bindingContext.Model = objectBinderLambda(valueProvider);
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
            return Task.CompletedTask;
        }

        private Func<IValueProvider, ProductsParameters> BuildParametersBinderLambda(Type parametersType)
        {
            if(bindParametersFuncsCash.TryGetValue(parametersType, out var func))
                return func;

            var valueProvider = Expression.Parameter(typeof(IValueProvider), "valueProvider");

            var block = GetBlockAssignExpression(valueProvider, parametersType);
            var lambda = Expression.Lambda<Func<IValueProvider, ProductsParameters>>(block, valueProvider);
            var returnFunc = lambda.Compile();
            bindParametersFuncsCash.Add(parametersType, returnFunc);
            return returnFunc;
        }

        private BlockExpression GetBlockAssignExpression(Expression valueProvider, Type parametersType)
        {
            var newExp = Expression.New(parametersType);
            var productParamtersVar = Expression.Variable(parametersType, "ProductParameters");
            var assignList = new List<Expression>()
            {
                Expression.Assign(productParamtersVar, newExp)
            };

            foreach (var p in parametersType.GetProperties())
            {
                if (p.Name == "CategoryType")
                    continue;
                var callGetValue = Expression.Call(valueProvider, getValueMethod, Expression.Constant(p.Name));
                var firstValue = Expression.Property(callGetValue, "FirstValue");
                var valueIsNotNull = Expression.NotEqual(firstValue, Expression.Constant(null, typeof(string)));
                var underluingType = Nullable.GetUnderlyingType(p.PropertyType);
                var propType = underluingType ?? p.PropertyType;

                if (TryGetNumberParseMethod(propType, out var parseMethod))
                {
                    var callParse = Expression.Call(parseMethod, firstValue);
                    Expression value = underluingType != null ? Expression.Convert(callParse, typeof(int?)) : callParse;
                    var assign = Expression.Assign(Expression.Property(productParamtersVar, p.Name), value);
                    assignList.Add(Expression.IfThen(valueIsNotNull, assign));
                } 
                else if(propType == typeof(string))
                {
                    assignList.Add(Expression.IfThen(valueIsNotNull,
                    Expression.Assign(Expression.Property(productParamtersVar, p.Name), firstValue)));
                }
                else if (propType == typeof(DateTime))
                {
                    var callParse = Expression.Call(dateTimeParceMethod, firstValue);
                    var assign = Expression.Assign(Expression.Property(productParamtersVar, p.Name), callParse);
                    assignList.Add(Expression.IfThen(valueIsNotNull, assign));
                }
                else if (propType == typeof(DateOnly))
                {
                    var callParse = Expression.Call(dateOnlyParseMethod, firstValue);
                    var assign = Expression.Assign(Expression.Property(productParamtersVar, p.Name), callParse);
                    assignList.Add(Expression.IfThen(valueIsNotNull, assign));
                }
                else
                {
                    throw new NotImplementedException($"Doesn't have the implementation for a parameter property of type: {p.PropertyType.Name}");
                }
            }
            assignList.Add(productParamtersVar);
            return Expression.Block(variables: new[] { productParamtersVar }, assignList);
        }

        Type GetParametersType(string category)
        {
            if (parametersTypeCash.TryGetValue(category, out var parameters))
                return parameters;

            string categoryParametersName = category + "Parameters";

            Type categoryParametersType = Type.GetType($"Shared.RequestFeatures.ProductsParameters.{categoryParametersName}, Shared", false) ??
                throw new BadRequestException($"The searched category parameters: \"{categoryParametersName}\" not found. " +
                $"searched for the path \"Shared.RequestFeatures.ProductsParameters.{categoryParametersName}\"");

            parametersTypeCash.Add(category, categoryParametersType);
            return categoryParametersType;
        }

        bool TryGetNumberParseMethod(Type type, out MethodInfo parseMethod)
        {
            parseMethod = null;

            if (type == typeof(int))
                parseMethod = intParseMethod;
            else if (type == typeof(long))
                parseMethod = longParseMethod;
            else if (type == typeof(float))
                parseMethod = floatParseMethod;
            else if (type == typeof(double))
                parseMethod = doubleParseMethod;
            else if (type == typeof(decimal))
                parseMethod = decimalParseMethod;

            if (parseMethod == null)
                return false;
            return true;
        }
    }
}
