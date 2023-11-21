using Entities.Models.Categories;
using Shared.RequestFeatures.ParametersAttributes;

namespace Shared.RequestFeatures.ProductsParameters
{
    [OrderByColumns(nameof(Laptops.Memory), nameof(Laptops.RAM))]
    public class LaptopsParameters : ProductsParameters
    {
        [SearchParameter("Brand", ExactMatch = true)]
        [SearchValues("Apple", "Asus", "Lenovo")]
        public string? SearchBrand { get; set; }

        [SearchParameter("Model", ExactMatch = true)]
        [SearchValues("Yoga C940", "Yoga C740", "Legion Y740", "Legion Y540", "Ideapad 330", "Ideapad S340", "Ideapad Flex 5", "ThinkBook 14", "ThinkBook 15", "Chromebook Duet",
            DependentOnParameter = nameof(SearchBrand), DependentOnValues = new[] {"Lenovo"})]
        [SearchValues("ZenBook 14", "ZenBook Pro Duo", "VivoBook S15", "VivoBook Flip 14", "ROG Zephyrus G14", "ROG Zephyrus Duo 15", "TUF Gaming A15", "TUF Gaming F15",
            DependentOnParameter = nameof(SearchBrand), DependentOnValues = new[] { "Asus" })]
        [SearchValues("MacBook Air", "MacBook Pro (16-inch)", "MacBook", "MacBook Pro",
            DependentOnParameter = nameof(SearchBrand), DependentOnValues = new[] { "Apple" })]
        public string? SearchModel { get; set; }

        [SearchParameter("Operating System", ExactMatch = true)]
        [SearchValues("Windows 10", "macOs", "Windows 9", "Windows 8", "Windows 11", "Linux 3.0", "Linux 3.10", "Linux 3.18", "Linux 4.0", "Linux 4.4", "Linux 4.8", "Linux 4.14", "Linux 4.19", "Linux 5.0", "Linux 5.4", "Linux 5.10", "Linux 5.15",
            DependentOnParameter = nameof(SearchBrand), DependentOnValues = new[] { "Lenovo", "Asus" })]
        [SearchValues("OS X 10.7 (Lion)", "OS X 10.8 (Mountain Lion)", "OS X 10.9 (Mavericks)", "OS X 10.10 (Yosemite)", "OS X 10.11 (El Capitan)", "macOS 10.12 (Sierra)", "macOS 10.13 (High Sierra)", "macOS 10.14 (Mojave)", "macOS 10.15 (Catalina)", "macOS 11.0 (Big Sur)", "macOS 12.0 (Monterey)", 
            DependentOnParameter = nameof(SearchBrand), DependentOnValues = new[] { "Apple" })]
        public string? SearchOperatingSystem { get; set; } = "";

        [ParameterName("Memory")]
        [SearchValues(128, 256, 512, 1024)]
        public int? SearchMemory { get; set; }

        [SearchParameter("Video Card", ExactMatch = true)]
        [SearchValues("NVIDIA GeForce GTX 1650 Mobile", "NVIDIA GeForce MX450", "AMD Radeon RX Vega 8", "NVIDIA Quadro T1000",
        "AMD Radeon RX 5500M", "Intel UHD Graphics Xe G4", "AMD Radeon RX 680M")]
        public string? SearchVideoCard { get; set; } = "";

        public override Type? CategoryType => typeof(Laptops);
    }
}
