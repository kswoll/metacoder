using Metacoder.Interfaces;
using Microsoft.CodeAnalysis;

namespace Metacoder.Host.TypeWrappers
{
    public struct AssemblyWrapper : IAssembly
    {
        private IAssemblySymbol symbol;

        public AssemblyWrapper(IAssemblySymbol symbol)
        {
            this.symbol = symbol;
        }

        public string Name
        {
            get { return symbol.Name; }
        }
    }
}