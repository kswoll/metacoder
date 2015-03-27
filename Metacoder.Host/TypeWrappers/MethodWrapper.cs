using System.Collections.Generic;
using System.Linq;
using Metacoder.Interfaces;
using Microsoft.CodeAnalysis;

namespace Metacoder.Host.TypeWrappers
{
    public struct MethodWrapper : IMethod
    {
        private Compilation compilation;
        private IMethodSymbol method;

        public MethodWrapper(Compilation compilation, IMethodSymbol method)
        {
            this.compilation = compilation;
            this.method = method;
        }

        public string Name
        {
            get { return method.Name; }
        }

        public IEnumerable<IAttribute> Attributes
        {
            get { return SymbolHelper.GetAttributes(compilation, method); }
        }

        public IType ReturnType
        {
            get { return new TypeWrapper(compilation, method.ReturnType); }
        }

        public IEnumerable<IParameter> Parameters
        {
            get
            {
                var compilation = this.compilation;
                return method.Parameters.Select(x => (IParameter)new ParameterWrapper(compilation, x));
            }
        }
    }
}