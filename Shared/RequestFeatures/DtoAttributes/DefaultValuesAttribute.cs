namespace Shared.RequestFeatures.DtoAttributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class DefaultValuesAttribute : Attribute
    {
        public bool OnlyDefaultValues;
        public object[] DefaultValues;
        public DefaultValuesAttribute(bool defaultOnly, params object[] values) 
        {
            OnlyDefaultValues = defaultOnly;
            DefaultValues = values;
        }
    }
}
