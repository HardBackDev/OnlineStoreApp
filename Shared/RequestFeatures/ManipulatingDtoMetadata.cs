using System.Dynamic;

namespace Shared.RequestFeatures
{
    public class ManipulatingDtoMetadata
    {
        public ExpandoObject ManipulatinObject { get; set; } = new();
        public Dictionary<string, object[]> PropertiesValues { get; set; } = new();
        public HashSet<string> OnlyDefaultValuesProperties { get; set; } = new();

    }
}
