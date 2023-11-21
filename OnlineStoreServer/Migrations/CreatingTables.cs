using Dapper;
using Entities.Models;
using FluentMigrator;
using Repository;
using Shared.utilities;
using System.Text;

namespace OnlineStoreServer.Migrations
{
    [Migration(1)]
    public class CreatingTables : Migration
    {
        readonly CreatingCategories _creatingCategories;
        readonly DapperContext _context;

        public CreatingTables(CreatingCategories creatingCategories, DapperContext context)
        {
            _creatingCategories = creatingCategories;
            _context = context;
        }

        public override void Down()
        {
            var sqlCommandBuilder = new StringBuilder();
            
            var connection = _context.CreateConnection();
            var categories = connection.Query<string>("SELECT Name FROM Categories");

            foreach (var category in categories)
            {
                sqlCommandBuilder.AppendLine($"DROP TABLE {category}");
            }
            sqlCommandBuilder.Append($"""
                DROP TABLE {Tables.CartsProductsTable};
                DROP TABLE [AspNetRoleClaims];
                DROP TABLE [AspNetUserClaims];
                DROP TABLE [AspNetUserLogins];
                DROP TABLE [AspNetUserRoles];
                DROP TABLE [AspNetUserTokens];
                DROP TABLE [AspNetRoles];
                DROP TABLE [AspNetUsers];
                DROP TABLE {Tables.CategoryTable}
                """);
            Execute.Sql(sqlCommandBuilder.ToString());
        }

        public override void Up()
        {
            Execute.Sql(File.ReadAllText("./IdentityTables.sql"));

            Create.Table(Tables.CategoryTable)
                .WithColumn(nameof(Category.Name)).AsString(255).NotNullable().PrimaryKey().Unique()
                .WithColumn(nameof(Category.PhotoUrl)).AsString(int.MaxValue).Nullable();

            Create.Table(Tables.CartsProductsTable)
                .WithColumn(nameof(CartsProducts.ProductId)).AsGuid().NotNullable()
                .WithColumn(nameof(CartsProducts.UserId)).AsString(450).ForeignKey("AspNetUsers", "Id").NotNullable();

            Execute.Sql($"""
                ALTER TABLE {Tables.CartsProductsTable}
                ADD CONSTRAINT UserId_ProductId PRIMARY KEY ({nameof(CartsProducts.ProductId)}, {nameof(CartsProducts.UserId)});
                """);

            foreach(var query in _creatingCategories.GetCreatingCategoriesQuery())
                Execute.Sql(query.ToString());
        }
    }
}
