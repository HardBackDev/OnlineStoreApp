using Shared.Dtos.ProductDtos;
using Shared.RequestFeatures.DtoAttributes;
using Shared.RequestFeatures.ParametersAttributes;

namespace Shared.Dto.ProductCategoriesDtos.LaptopsDtos
{
    public record LaptopsDto : ProductDto
    {
        public string Brand { get; set; }
        public string Model { get; set; }

        [PropertyName("Screen Diagonal (inch)")]
        public float ScreenDiagonal { get; set; }
        [PropertyName("Screen Resolution")]
        public string ScreenResolution { get; set; }

        [PropertyName("RAM (gb)")]
        public int RAM { get; set; }
        [PropertyName("Memory (gb)")]
        public int Memory { get; set; }
        [PropertyName("Operating System")]
        public string OperatingSystem { get; set; }
        [PropertyName("Video Card")]
        public string VideoCard { get; set; }
    }
}
