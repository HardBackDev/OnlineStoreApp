namespace Shared.RequestFeatures.ParametersAttributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class OrderByColumnsAttribute : Attribute
    {
        public string[] Columns { get; set; }

        public OrderByColumnsAttribute(params string[] columns)
        {
            Columns = columns;
        }
    }
}
