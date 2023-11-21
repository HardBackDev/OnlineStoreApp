
namespace Shared.RequestFeatures.ParametersAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ParameterNameAttribute : Attribute
    {
        public string ParameterName { get; set; }
        public ParameterNameAttribute(string name)
        {
            ParameterName = name;
        }
    }
}
