using Shared.RequestFeatures.ParametersAttributes;
using Shared.RequestFeatures.ProductsParameters;
using System.Reflection;

namespace Repository.Queries
{
    public static class ParametersQueryBuilder
    {
        static Dictionary<Type, string> ParametersQueriesHash = new();

        public static string BuildParametersQuery(ProductsParameters parameters)
        {
            var parametersType = parameters.GetType();
            if(ParametersQueriesHash.TryGetValue(parametersType, out var query))
            {
                return query;
            }

            var props = parametersType.GetProperties();
            var rangeConditions = new HashSet<string>();
            var conditions = new List<string>();


            foreach (var prop in props)
            {
                if (prop.Name.Contains("search", StringComparison.OrdinalIgnoreCase))
                {
                    conditions.Add(GetSearchCondition(prop.Name, prop.Name.Substring("search".Length), prop));

                }
                else if (prop.Name.StartsWith("min", StringComparison.OrdinalIgnoreCase) ||
                    prop.Name.StartsWith("max", StringComparison.OrdinalIgnoreCase))
                {
                    string targetColumn = prop.Name.Substring(3);
                    string rangeCondition = GetRangeCondition("Min" + targetColumn, "Max" + targetColumn, targetColumn);
                    if (rangeConditions.Add(rangeCondition))
                    {
                        conditions.Add(rangeCondition);
                    }
                }
            }
            var filterByParametersQuery = string.Join(" AND ", conditions);
            ParametersQueriesHash.Add(parametersType, filterByParametersQuery);

            return filterByParametersQuery;
        }

        private static string GetSearchCondition(string searchVariable, string targetСolumn, PropertyInfo parameter)
        {
            bool searchExact = false;
            if (parameter.IsDefined(typeof(SearchParameterAttribute)))
            {
                var attribute = (SearchParameterAttribute)Attribute.GetCustomAttribute(parameter, typeof(SearchParameterAttribute));
                searchExact = attribute.ExactMatch;
            }

            if (parameter.PropertyType == typeof(string))
            {
                string checkOnNullOrEmpty = $"""
                    ((@{searchVariable} LIKE N'') OR
                    (@{searchVariable} IS NULL))
                    """;
                if (!searchExact)
                {
                    return $"""
                    ({checkOnNullOrEmpty} OR
                    (CHARINDEX(@{searchVariable}, LOWER({targetСolumn}))) > 0)
                    """;
                }
                else
                {
                    return $"""
                    ({checkOnNullOrEmpty} OR
                    ({targetСolumn} = @{searchVariable}))
                    """;
                }
            }
            else
                return $"""
                    ({targetСolumn} = @{searchVariable} OR @{searchVariable} IS NULL)
                    """;
        } 

        private static string GetRangeCondition(string minVariable, string maxVariable, string targetColumn) => $"""
            ((@{minVariable} IS NULL OR @{maxVariable} IS NULL) OR
            ({targetColumn} >= @{minVariable} AND {targetColumn} <= @{maxVariable}))
            """;
    }
}
