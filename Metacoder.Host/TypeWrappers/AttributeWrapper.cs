using System.Collections.Generic;
using System.Linq;
using Metacoder.Interfaces;
using Microsoft.CodeAnalysis;

namespace Metacoder.Host.TypeWrappers
{
    public struct AttributeWrapper : IAttribute
    {
        private Compilation compilation;
        private AttributeData attribute;

        public AttributeWrapper(Compilation compilation, AttributeData attribute)
        {
            this.compilation = compilation;
            this.attribute = attribute;
        }

        public IType Type
        {
            get { return new TypeWrapper(compilation, attribute.AttributeClass); }
        }

        public IMethod Constructor
        {
            get { return new MethodWrapper(compilation, attribute.AttributeConstructor); }
        }

        public object[] ConstructorArguments
        {
            get { return attribute.ConstructorArguments.Select(x => x.Value).ToArray(); }
        }

        public Dictionary<string, object> NamedArguments
        {
            get { return attribute.NamedArguments.ToDictionary(x => x.Key, x => x.Value.Value); }
        }
    }
}