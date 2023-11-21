using Entities.Models.Categories;
using Shared.RequestFeatures.ParametersAttributes;

namespace Shared.RequestFeatures.ProductsParameters
{
    public class SmartphonesParameters : ProductsParameters
    {
        [SearchParameter("Brand", ExactMatch = true)]
        [SearchValues("Apple", "Samsung", "Huawei", "Xiaomi")]
        public string? SearchBrand { get; set; }

        [SearchParameter("Model", ExactMatch = true)]
        #region Models search values
        [SearchValues("iPhone X", "iPhone XR", "iPhone XS", "iPhone XS Max", "iPhone 11", "iPhone 11 Pro", "iPhone 11 Pro Max", "iPhone SE (2nd generation)", "iPhone 12 mini", "iPhone 12", "iPhone 12 Pro", "iPhone 12 Pro Max", 
            DependentOnParameter = nameof(SearchBrand), DependentOnValues = new[] { "Apple" })]
        [SearchValues("Samsung Galaxy A10", "Samsung Galaxy A20", "Samsung Galaxy A30", "Samsung Galaxy A40", "Samsung Galaxy A50", "Samsung Galaxy A60", "Samsung Galaxy A70", "Samsung Galaxy A80", "Samsung Galaxy A90", 
            DependentOnParameter = nameof(SearchBrand), DependentOnValues = new[] { "Samsung" })]
        [SearchValues("Huawei MatePad Pro", "Huawei Mate Xs", "Huawei mate Xs2", "Huawei Mate 40", "Huawei Mate 40 Pro", "Huawei Mate 40 Pro+", "Huawei Mate 40 RS", "Huawei Mate 40E", "Huawei Mate 40E 4G", "Huawei Mate 40 Pro 4G", "Huawei Mate 50", "Huawei Mate 50 Pro", "Huawei Mate 60", "Huawei Mate 60 Pro", "Huawei Mate 50 RS Porsche Design", "Huawei Mate X2", "Huawei Mate X2 4G", "Huawei Mate X3", 
            DependentOnParameter = nameof(SearchBrand), DependentOnValues = new[] {"Huawei"})]
        [SearchValues("Xiaomi Mi 11", "Xiaomi Mi 11 Ultra", "Xiaomi Mi Note 10", "Xiaomi Mi A3", "Redmi Note 8 Pro", "Xiaomi Mi 10 Ultra", "Xiaomi Mi A1", "Xiaomi Mi 9T Pro", "Xiaomi Mi MIX", 
            DependentOnParameter = nameof(SearchBrand), DependentOnValues = new[] { "Xiaomi" })]
        #endregion
        public string? SearchModel { get; set; } = "";

        [SearchParameter("Operating System", ExactMatch = true)]
        #region Operating system search values
        [SearchValues("iOS 8.0", "iOS 9.0", "iOS 10.0", "iOS 11.0", "iOS 12.0", "iOS 13.0", "iOS 14.0", "iOS 15.0", 
            DependentOnParameter = nameof(SearchBrand), DependentOnValues = new[] {"Apple"})]
        [SearchValues("Android Gingerbread", "Android Ice Cream Sandwich", "Android Jelly Bean", "Android KitKat", "Android Lollipop", "Android Marshmallow", "Android Nougat", "Android Oreo", "Android Pie", "Android 10", "Android 11", "Android 12", 
            DependentOnParameter = nameof(SearchBrand), DependentOnValues = new[] { "Samsung" })]
        [SearchValues("HarmonyOs 1.0", "HarmonyOs 2.0", "HarmonyOs 3.0", "HarmonyOs 3.1", "HarmonyOs 4.0", 
            DependentOnParameter = nameof(SearchBrand), DependentOnValues = new[] { "Huawei" })]
        [SearchValues("MIUI 6", "MIUI 7", "MIUI 8", "MIUI 9", "MIUI 10", "MIUI 11", "MIUI 12", 
            DependentOnParameter = nameof(SearchBrand), DependentOnValues = new[] { "Xiaomi" })]
        #endregion
        public string? SearchOperatingSystem { get; set; } = "";

        [SearchParameter("CPU", ExactMatch = true)]
        #region CPU search values
        [SearchValues("Apple A8", "Apple A9", "Apple A10 Fusion", "Apple A11 Bionic", "Apple A12 Bionic", "Apple A13 Bionic", "Apple A14 Bionic", 
            DependentOnParameter = nameof(SearchBrand), DependentOnValues = new[] {"Apple"})]
        [SearchValues("Samsung Exynos 1380", "Samsung Exynos 1330", "Samsung Exynos 2200", "Samsung Exynos 1280", "Samsung Exynos 2100", "Samsung Exynos 1080","Samsung Exynos 880", "Samsung Exynos 850", "Samsung Exynos 990", "Samsung Exynos 980",
            DependentOnParameter = nameof(SearchBrand), DependentOnValues = new[] { "Samsung" })]
        [SearchValues("HiSilicon Kirin 9000", "HiSilicon Kirin 990", "HiSilicon Kirin 980", "HiSilicon Kirin 810/820", "HiSilicon Kirin 970", "HiSilicon Kirin 710", "HiSilicon Kirin 710A",
            DependentOnParameter = nameof(SearchBrand), DependentOnValues = new[] { "Huawei" })]
        [SearchValues("Qualcomm Snapdragon 695 5G", "Qualcomm QCM6490", "Qualcomm Snapdragon 888+", "Qualcomm Snapdragon 888", "Qualcomm Snapdragon 690 5G",
            DependentOnParameter = nameof(SearchBrand), DependentOnValues = new[] { "Xiaomi" })]
        #endregion
        public string? SearchCPU { get; set; }

        [SearchParameter("Memory")]
        [SearchValues(64, 128, 256, 512)]
        public int? SearchMemory { get; set; }

        public override Type? CategoryType => typeof(Smartphones);
    }
}