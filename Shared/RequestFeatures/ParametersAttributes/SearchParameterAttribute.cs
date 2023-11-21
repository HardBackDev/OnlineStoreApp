namespace Shared.RequestFeatures.ParametersAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SearchParameterAttribute : ParameterNameAttribute
    {
        public SearchParameterAttribute(string name) : base(name)
        {
        }
        public bool ExactMatch { get; set; }
    }
}
