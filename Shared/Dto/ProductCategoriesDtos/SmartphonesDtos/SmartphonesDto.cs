using Shared.Dtos.ProductDtos;
using Shared.RequestFeatures.DtoAttributes;
using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.ProductCategoriesDtos.SmartphonesDtos
{
    public record SmartphonesDto : ProductDto
    {
        public string Brand { get; set; }
        [PropertyName("Model")]
        public string Model { get; init; }
        [PropertyName("Operating System")]
        public string OperatingSystem { get; init; }
        [PropertyName("Memory (gb)")]
        public int Memory { get; init; }
        [PropertyName("RAM (gb)")]
        public int RAM { get; init; }
        [PropertyName("CPU")]
        public string CPU { get; init; }
        [PropertyName("Height (mm)")]
        public float Height { get; set; }
        [PropertyName("Width (mm)")]
        public float Width { get; set; }
        [PropertyName("Thickness (mm)")]
        public float Thickness { get; set; }

    }
}
