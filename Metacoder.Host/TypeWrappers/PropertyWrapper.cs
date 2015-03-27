using System.Collections.Generic;
using System.Linq;
using Metacoder.Interfaces;
using Microsoft.CodeAnalysis;

namespace Metacoder.Host.TypeWrappers
{
    public struct PropertyWrapper : IProperty
    {
        private Compilation compilation;
        private IPropertySymbol property;

        public PropertyWrapper(Compilation compilation, IPropertySymbol property)
        {
            this.compilation = compilation;
            this.property = property;
        }

        public string Name
        {
            get { return property.Name; }
        }

        public IEnumerable<IAttribute> Attributes
        {
            get { return SymbolHelper.GetAttributes(compilation, property); }
        }

        public IType Type
        {
            get { return new TypeWrapper(compilation, property.Type); }
        }

        public bool IsIndexer
        {
            get { return property.IsIndexer; }
        }

        public IMethod Getter
        {
            get { return property.GetMethod == null ? null : (IMethod)new MethodWrapper(compilation, property.GetMethod); }
        }

        public IMethod Setter
        {
            get { return property.SetMethod == null ? null : (IMethod)new MethodWrapper(compilation, property.SetMethod); }
        }

        public IEnumerable<IParameter> Parameters
        {
            get
            {
                var compilation = this.compilation;
                return property.Parameters.Select(x => (IParameter)new ParameterWrapper(compilation, x));
            }
        }
    }
}