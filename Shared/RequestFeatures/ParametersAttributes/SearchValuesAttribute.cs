namespace Shared.RequestFeatures.ParametersAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class SearchValuesAttribute : Attribute
    {
        public object[] SearchingValues;
        public string? DependentOnParameter;
        public object[]? DependentOnValues;

        public SearchValuesAttribute(params object[] searchingValues)
        {
            SearchingValues = searchingValues;
        }
    }
}
