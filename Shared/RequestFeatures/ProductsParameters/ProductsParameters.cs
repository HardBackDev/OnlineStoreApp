using Entities.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shared.RequestFeatures.ParametersAttributes;

namespace Shared.RequestFeatures.ProductsParameters
{
    public class ProductsParameters : RequestParameters
    {
        public virtual Type? CategoryType => typeof(Product);
        public string? SearchTitle { get; set; } = "";
        public int MinPrice { get; set; } = 0;
        public int MaxPrice { get; set; } = int.MaxValue;
    }
}
