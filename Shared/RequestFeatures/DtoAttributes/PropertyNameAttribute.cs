namespace Shared.RequestFeatures.DtoAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PropertyNameAttribute : Attribute
    {
        public string Name { get; set; }

        public PropertyNameAttribute(string name)
        {
            Name = name;
        }
    }
}
