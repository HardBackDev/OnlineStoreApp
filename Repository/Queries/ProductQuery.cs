using Entities.Models;
using Shared.Dtos.ProductDtos;
using Shared.RequestFeatures;
using Shared.RequestFeatures.ProductsParameters;
using Shared.utilities;
using System.Text;

namespace Repository.Queries
{
    public static class ProductQuery
    {
        public const string ProductTable = nameof(Product);
        public const string ProductId = nameof(Product.Id);
        public const string ProductPhoto = nameof(Product.PhotoUrl);
        public const string ProductPrice = nameof(Product.Price);
        public const string ProductTitle = nameof(Product.Title);
        public const string ProductCategory = nameof(Product.Category);

        public static string DeleteProduct(string category) => $"""
            DELETE FROM {category}
            WHERE {ProductId} = @Id
            """;

        public static string SelectProductById(string category) => $"""
            SELECT * FROM {category} p
            WHERE p.{ProductId} = @Id
            """;

        public static string CheckProductInCart()
        {
            string selectByCartCondition = $"""
                JOIN {nameof(CartsProducts)} cp
                ON p.{ProductId} = cp.{nameof(CartsProducts.ProductId)}
                WHERE cp.{nameof(CartsProducts.UserId)} = @userId
                AND cp.{nameof(CartsProducts.ProductId)} = @productId
                """;

            string checkProductInCartQuery = GetSelectAllProductsQueryWithExpression(selectByCartCondition, alias: "p");

            return checkProductInCartQuery;
        }

        public static string GetSelectProductsByCartId(ProductsParameters parameters)
        {
            string parametersCondition = ParametersQueryBuilder.BuildParametersQuery(parameters);
            string selectByCartCondition = $"""
                JOIN {nameof(CartsProducts)} cp
                ON p.{ProductId} = cp.{nameof(CartsProducts.ProductId)}
                WHERE cp.{nameof(CartsProducts.UserId)} = @Id
                AND
                {parametersCondition}
                """;

            string selectCountRows = GetSelectAllProductsQueryWithExpression
                (
                expression: selectByCartCondition,
                selectStatement: $"Count({ProductId}) As countRows FROM Products",
                alias: "p"
                );
            string selectSummaryPrice = GetSelectAllProductsQueryWithExpression
                (
                expression: selectByCartCondition,
                selectStatement: $"SUM({ProductPrice}) As summaryPrice FROM Products",
                alias: "p"
                );

            string selectByCartQuery = GetSelectAllProductsQueryWithExpression(expression: selectByCartCondition, alias: "p");

            return $"""
                {selectCountRows};
                {selectSummaryPrice};
                {selectByCartQuery}
                {GetPaggingQuery(parameters)}
                """;
        }

        public static string GetUpdateProduct(string category, ProductForManipulating product)
        {
            var dtoType = product.GetType();
            var properties = dtoType.GetProperties();
            var setQueryBuilder = new StringBuilder();

            foreach (var p in properties)
            {
                setQueryBuilder.Append($"{p.Name} = @{p.Name}, ");
            }

            string setStatement = setQueryBuilder.ToString().Trim(',', ' ');

            return $"""
            UPDATE {category} SET
            {setStatement}
            WHERE {ProductId} = @Id
            """;
        }

        public static string GetInsertProduct(string category, ProductForManipulating product)
        {
            var dtoType = product.GetType();
            var properties = dtoType.GetProperties();
            var createStatementBuilder = new StringBuilder();
            var valuesBuilder = new StringBuilder();

            foreach (var p in properties)
            {
                createStatementBuilder.Append($"{p.Name}, ");
                valuesBuilder.Append($"@{p.Name}, ");
            }
            string valuesStatement = valuesBuilder.ToString().Trim(',', ' ');
            string createStatement = createStatementBuilder.ToString().Trim(',', ' ');

            return $"""
            INSERT INTO {category} 
            ({createStatement}, {ProductCategory}, {ProductId})
            OUTPUT inserted.{ProductId}
            VALUES ({valuesStatement}, default, default);
            """;
        }

        public static string GetSelectProductsByCategory(string category, ProductsParameters parameters)
        {
            string parametersCondition = ParametersQueryBuilder.BuildParametersQuery(parameters);

            string pagingExpression = GetPaggingQuery(parameters);

            if (category.Equals("Products", StringComparison.OrdinalIgnoreCase))
            {
                var selectCountRowsFromTables = GetSelectAllProductsQueryWithExpression
                    (
                    expression: $"WHERE {parametersCondition}",
                    selectStatement: $"Count({ProductId}) As countRows FROM Products"
                    );

                var selectProductByParameters = GetSelectAllProductsQueryWithExpression
                    (
                    expression: $"WHERE {parametersCondition} {pagingExpression}",
                    alias: "p"
                    );

                return $"{selectCountRowsFromTables}; {selectProductByParameters}";
            }
            var selectCoundRowsFromTable = $"""
                SELECT COUNT(p.{ProductId})
                FROM {category} AS p
                WHERE
                {parametersCondition}
                """;

            string selectByCategory = $"""
                {selectCoundRowsFromTable};
                SELECT * FROM {category} AS p
                WHERE
                {parametersCondition}
                {pagingExpression}
                """;

            return selectByCategory;
        }

        static HashSet<(Type, string)> columnsCache = new();
        private static string GetPaggingQuery(ProductsParameters parameters)
        {
            var orderStatement = parameters.OrderBy.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            
            var orderDirection = orderStatement.Length == 2 ? orderStatement[1] : "";

            if (!orderDirection.Equals("desc", StringComparison.OrdinalIgnoreCase))
                orderDirection = "asc";

            string orderColumn;
            if (orderStatement.Length == 0)
            {
                orderColumn = "Price";
            }
            else if (columnsCache.TryGetValue((parameters.CategoryType, orderStatement[0]), out var column))
            {
                orderColumn = column.Item2;
            }
            else
            {
                if (parameters.CategoryType.GetProperties().Any(p => p.Name.Equals(orderStatement[0], StringComparison.OrdinalIgnoreCase)))
                {
                    orderColumn = orderStatement[0];
                    columnsCache.Add((parameters.CategoryType, orderColumn.ToUpper()));
                }
                else
                {
                    orderColumn = "Price";
                }
            }


            return $"""
                ORDER BY [{orderColumn}] {orderDirection}
                OFFSET {(parameters.PageNumber - 1) * parameters.PageSize} ROWS FETCH NEXT {parameters.PageSize} ROWS ONLY
                """;
        }

        public static string GetSelectAllProductsQueryWithExpression(string expression = "", string selectStatement = "", string alias = "")
        {
            var queryBuilder = new StringBuilder("""
                WITH Products AS (

                """);
            var categoryNames = Tables.CategoriesTables
                .Select(t => t.Name)
                .ToList();
            string construct = string.IsNullOrEmpty(alias) ? "" : $"{alias}.";
            string productsColumns =  $"{construct}{ProductId}, {construct}{ProductPhoto}, {construct}{ProductPrice}, {construct}{ProductCategory}, {construct}{ProductTitle}";
            
            foreach (var tableName in categoryNames)
            {
                if(construct == "")
                    queryBuilder.AppendLine($"SELECT {productsColumns} FROM {tableName} UNION ALL ");
                else
                    queryBuilder.AppendLine($"SELECT {productsColumns} FROM {tableName} {alias} UNION ALL ");
            }
            var query = queryBuilder.ToString();
            int lastUnionIndex = query.LastIndexOf(" UNION ALL ");
            query = query.Remove(lastUnionIndex);
            string selectProducts = string.IsNullOrEmpty(selectStatement) ? $"{productsColumns} FROM Products" : selectStatement;

            query +=  $"""
            ) 
            SELECT {selectProducts} {alias}
            {expression}
            """;

            return query;
        }
    }
}
