using Shared.RequestFeatures.ParametersAttributes;
using Shared.RequestFeatures.ParametersModels;
using System.Dynamic;

namespace Shared.RequestFeatures
{
    public class ParametersMetaData
    {
        public ExpandoObject Parameters { get; set; } = new();
        public Dictionary<string, object[]> ParametersSearchValues = new();
        public Dictionary<string, string> ParametersNames = new();
        public List<DependentSearchParameter> DependentSearchValues = new();
        public List<string>? OrderByColumns { get; set; } = new();
    }
}
