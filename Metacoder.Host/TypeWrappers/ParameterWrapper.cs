using System.Collections.Generic;
using Metacoder.Interfaces;
using Microsoft.CodeAnalysis;

namespace Metacoder.Host.TypeWrappers
{
    public struct ParameterWrapper : IParameter
    {
        private Compilation compilation;
        private IParameterSymbol parameter;

        public ParameterWrapper(Compilation compilation, IParameterSymbol parameter) 
        {
            this.compilation = compilation;
            this.parameter = parameter;
        }

        public string Name
        {
            get { return parameter.Name; }
        }

        public IEnumerable<IAttribute> Attributes
        {
            get { return SymbolHelper.GetAttributes(compilation, parameter); }
        }

        public IType Type
        {
            get { return new TypeWrapper(compilation, parameter.Type); }
        }
    }
}