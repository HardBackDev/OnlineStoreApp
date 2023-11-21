using Entities.Models;
using Shared.utilities;

namespace Repository.Queries
{
    public static class CartQuery
    {
        public static string AddProductToCartQuery = $"""
            INSERT INTO {Tables.CartsProductsTable}
            ({nameof(CartsProducts.UserId)}, {nameof(CartsProducts.ProductId)})
            VALUES (@UserId, @ProductId)
            """;

        public static string DeleteProductFromCart = $"""
            DELETE FROM {Tables.CartsProductsTable}
            WHERE
            {nameof(CartsProducts.UserId)} = @UserId AND {nameof(CartsProducts.ProductId)} = @ProductId
            """;
    }
}
