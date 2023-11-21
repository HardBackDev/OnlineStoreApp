using Shared.Dtos.ProductDtos;
using Shared.RequestFeatures.DtoAttributes;
using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.ProductCategoriesDtos.LaptopsDtos
{
    public record LaptopsForManipulating : ProductForManipulating
    {
        [DefaultValues(true, "Apple", "Asus", "Lenovo")]
        public string Brand { get; set; }
        public string Model { get; set; }
        public float ScreenDiagonal { get; set; }
        [DefaultValues(true, "1366 x 768 (HD)", "1600 x 900 (HD+)", "1920 x 1080 (Full HD)", "2560 x 1440 (Quad HD or 2K)", "3200 x 1800", "3840 x 2160 (Ultra HD or 4K)", "5120 x 2880 (5K)", "6016 x 3384", "7680 x 4320 (8K)")]
        public string ScreenResolution { get; set; }

        [DefaultValues(true, 6,8,16,32,64)]
        public int? RAM { get; set; }

        [DefaultValues(true, 128, 256, 512, 1024)]
        public int? Memory { get; set; }

        [DefaultValues(true, "Windows 10", "macOs", "Windows 9", "Windows 8", "Windows 11", "Linux 3.0", "Linux 3.10", "Linux 3.18", "Linux 4.0", "Linux 4.4", "Linux 4.8", "Linux 4.14", "Linux 4.19", "Linux 5.0", "Linux 5.4", "Linux 5.10", "Linux 5.15", "OS X 10.7 (Lion)", "OS X 10.8 (Mountain Lion)", "OS X 10.9 (Mavericks)", "OS X 10.10 (Yosemite)", "OS X 10.11 (El Capitan)", "macOS 10.12 (Sierra)", "macOS 10.13 (High Sierra)", "macOS 10.14 (Mojave)", "macOS 10.15 (Catalina)", "macOS 11.0 (Big Sur)", "macOS 12.0 (Monterey)")]
        public string OperatingSystem { get; set; }

        [DefaultValues(false, "NVIDIA GeForce GTX 1650 Mobile", "NVIDIA GeForce MX450", "AMD Radeon RX Vega 8", "NVIDIA Quadro T1000",
            "AMD Radeon RX 5500M", "Intel UHD Graphics Xe G4", "AMD Radeon RX 680M")]
        public string VideoCard { get; set; }
    }
}
