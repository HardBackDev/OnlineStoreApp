namespace Shared.RequestFeatures.ParametersModels
{
    public class DependentSearchParameter
    {
        public string Parameter { get; set; }
        public string DependentOnParameter { get; set; }
        public object[] DependentOnValues { get; set; }
        public object[] SearchValues { get; set; }
    }
}