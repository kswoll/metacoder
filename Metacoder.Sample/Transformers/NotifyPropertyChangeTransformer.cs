using System.IO;
using System.Linq;
using System.Text;
using Metacoder.Interfaces;

namespace Metacoder.Sample.Transformers
{
    public class NotifyPropertyChangeTransformer : IMetacoder
    {
        private const string template = @"
namespace {0}
{{
    public partial class {1}
    {{
{2}
    }}
}}
";
        private const string propertyTemplate = @"        public {0} {1}
        {{
            get {{ return {2}; }}
            set {{ this.{2} = value; }}
        }}

";

        public void Transform(ITransformationContext context)
        {
            foreach (var modelType in context.ProjectTypes.Where(x => !x.IsAbstract && x.Is<AbstractModel>()))
            {
                var location = modelType.Locations.Single(x => !x.EndsWith(".Generated.cs"));
                var newLocation = location.Substring(0, location.LastIndexOf('.'));
                newLocation += ".Generated.cs";

                var propertiesBuilder = new StringBuilder();
                foreach (var propertyField in modelType.Members.OfType<IField>().Where(x => x.HasAttribute<PropertyAttribute>()))
                {
                    var propertyName = char.ToUpper(propertyField.Name[0]) + propertyField.Name.Substring(1);
                    propertiesBuilder.AppendFormat(propertyTemplate, propertyField.Type.FullName, propertyName, propertyField.Name);
                }

                var content = string.Format(template, modelType.Namespace, modelType.Name, propertiesBuilder);

                context.CreateOrUpdateFile(newLocation, content, Path.GetFileName(location));
            }
        }
    }
}