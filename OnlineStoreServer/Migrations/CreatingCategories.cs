using Dapper;
using Entities.Models;
using Entities.Models.ModelsAttributes;
using Repository;
using Shared.utilities;
using System.Reflection;
using System.Text;

namespace OnlineStoreServer.Migrations
{
    public class CreatingCategories
    {

        private readonly DapperContext _context;

        public CreatingCategories(DapperContext context) => _context = context;

        string selectAllTablesQuery = """
                SELECT name
                FROM sys.tables;
                """;

        Dictionary<Type, string> columnTypesDictionary = new()
        {
            {typeof(string), "[varchar](1000)"},
            {typeof(int), "[int]"},
            {typeof(double), "FLOAT" },
            {typeof(float), "FLOAT" },
            {typeof(decimal), "DECIMAL" },
            {typeof(DateOnly), "[date]" },
            {typeof(DateTime), "[datetime]" },
            {typeof(Guid), "[uniqueidentifier]" }
        };

        string defaultProductsColumnsCreation = $"""
            [{nameof(Product.Id)}] [uniqueidentifier] DEFAULT NEWID() PRIMARY KEY,
            [{nameof(Product.Price)}] [int] NOT NULL,
            [{nameof(Product.Title)}] [nvarchar](400) NOT NULL,
            [{nameof(Product.PhotoUrl)}] [nvarchar](MAX) NULL,
            """;

        public List<string> GetCreatingCategoriesQuery()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Info: Creating Categories Tables");
            Console.ResetColor();

            var categoriesToCreate = GetDontExistingCategories(Tables.CategoriesTables).ToList();
            if (categoriesToCreate.Count == 0)
            {
                Console.WriteLine("No new categories to create");
                return null;
            }
            var creatingQueries = new List<string>();
            foreach (var category in categoriesToCreate)
            {
                creatingQueries.Add(GetCategoryCreationQuery(category));
            }

            return creatingQueries;
        }

        IEnumerable<Type> GetDontExistingCategories(IEnumerable<Type> categories)
        {
            using var connection = _context.CreateMasterConnection();
            var existingTables = connection.Query<string>(selectAllTablesQuery).ToHashSet();

            return categories
                .Where(t => !existingTables.Contains(t.Name))
                .ToList();
        }

        string GetCategoryCreationQuery(Type productCategoryType)
        {
            var sqlCommandBuilder = new StringBuilder();
            sqlCommandBuilder.AppendLine($"CREATE TABLE {productCategoryType.Name} (");
            sqlCommandBuilder.AppendLine(defaultProductsColumnsCreation);

            foreach (var column in productCategoryType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                var dataType = columnTypesDictionary[column.PropertyType];
                sqlCommandBuilder.AppendLine($"[{column.Name}] {dataType} NOT NULL,");
            }
            sqlCommandBuilder.AppendLine($"""
                {nameof(Product.Category)} NVARCHAR(255) DEFAULT '{productCategoryType.Name}' NOT NULL

                FOREIGN KEY ({nameof(Product.Category)})
                REFERENCES Categories ({nameof(Category.Name)})
                ON UPDATE CASCADE
                """);
            sqlCommandBuilder.AppendLine(");");

            string categoryPhoto = "";
            if (productCategoryType.IsDefined(typeof(ProductCategoryPhotoAttribute), false))
            {
                var attribute = Attribute.GetCustomAttribute(productCategoryType, typeof(ProductCategoryPhotoAttribute)) as ProductCategoryPhotoAttribute;
                categoryPhoto = attribute.PhotoUrl;
            }

            sqlCommandBuilder.AppendLine($"INSERT INTO {Tables.CategoryTable} ({nameof(Category.Name)}, {nameof(Category.PhotoUrl)}) VALUES('{productCategoryType.Name}', '{categoryPhoto}')");

            return sqlCommandBuilder.ToString();
        }
    }
}
