
using Entities.Models.Categories;
using Shared.RequestFeatures.DtoAttributes;
using Shared.RequestFeatures.ParametersAttributes;

namespace Shared.RequestFeatures.ProductsParameters
{
    [OrderByColumns(nameof(Meats.DateCreated))]
    public class MeatsParameters : ProductsParameters
    {
        [ParameterName("Min Create Date")]
        public DateOnly MinDateCreated { get; set; } = DateOnly.Parse("1753-1-1");

        [ParameterName("Max Create Date")]
        public DateOnly MaxDateCreated { get; set; } = DateOnly.Parse("9999-12-31");

        [SearchValues("Beef", "Pork", "Chicken", "Lamb", "Turkey")]
        [ParameterName("Meat Type")]
        public string? SearchMeatType { get; set; } = "";

        public override Type? CategoryType => typeof(Meats);
    }
}
