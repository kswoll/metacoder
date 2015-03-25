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
    }
}