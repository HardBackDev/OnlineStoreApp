using Entities.Models;
using Entities.Models.Categories;
using System.Reflection;

namespace Shared.utilities
{
    /// <summary>
    /// stored all tables names
    /// </summary>
    public static class Tables
    {
        public static List<Type> CategoriesTables;
        static Tables()
        {
            CategoriesTables = Assembly.GetAssembly(typeof(Product))
                .GetTypes()
                .Where(t => t.Namespace.Equals("Entities.Models.Categories") && t.IsAssignableTo(typeof(Product)))
                .ToList();
        }

        public static string CategoryTable = "Categories";
        public static string CartsProductsTable = nameof(CartsProducts);
    }
}
