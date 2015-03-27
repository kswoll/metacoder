Metacoder
---------

Metacoder is a Roslyn based tool that allows you to write classes that will take C# code you've written and allow you to generate classes based on that.  

For example, you could declare fields in your class and generate the notify property changed properties around it.  To illustrate, let's look at a simple example that merely takes your fields and generates simple getters and setters for them in a generated partial class. 

First, your "template" class:

    public partial class TestModel
    {
        [Property]private string foo;
        [Property]private string bar;
        [Property]private string foobar;
    }

Here's we've defined an attribute, `PropertyAttribute` that is just a naked attribute we're using to mark the fields for which we want properties generated.

Next we need to define the generator.  This is a class that implements the interface `IMetacoder`:

    public class NotifyPropertyChangeMetacoder : IMetacoder
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
            foreach (var modelType in context.ProjectTypes.Where(x => !x.IsAbstract && x.Members.OfType<IField>().Any(y => y.HasAttribute<PropertyAttribute>())))
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

Finally, upon building your project, it will generate the following output (alongside your existing `TestModel`, adding it to the project if necessary):

    public partial class TestModel
    {
        public System.String Foo
        {
            get { return foo; }
            set { this.foo = value; }
        }

        public System.String Bar
        {
            get { return bar; }
            set { this.bar = value; }
        }

        public System.String Foobar
        {
            get { return foobar; }
            set { this.foobar = value; }
        }
    }
