using Entities.Models;
using Shared.utilities;

namespace Repository.Queries
{
    public static class CategoryQuery
    {
        public static string CategoryTable = Tables.CategoryTable;
        public static string CategoryName = nameof(Category.Name);
        public static string CategoryPhotoUrl = nameof(Category.PhotoUrl);

        public static string selectCategoriesByCatalogQuery = $"""
            SELECT * FROM {CategoryTable}
            """;

        public static string selectCategoryByNameQuery = $"""
            SELECT * FROM {CategoryTable}
            WHERE {CategoryName} = @Name
            """;

        public static string updateCategoryQuery = $"""
            UPDATE {Tables.CategoryTable} SET
            {CategoryName} = @{CategoryName},
            {CategoryPhotoUrl} = @{CategoryPhotoUrl}
            WHERE {CategoryName} = @updatedName
            """;
    }
}
