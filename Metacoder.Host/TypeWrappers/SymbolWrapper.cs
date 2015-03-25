using System.Collections.Generic;
using Metacoder.Interfaces;
using Microsoft.CodeAnalysis;

namespace Metacoder.Host.TypeWrappers
{
    public class SymbolWrapper
    {
        protected readonly Compilation compilation;
        private ISymbol symbol;

        public SymbolWrapper(Compilation compilation, ISymbol symbol)
        {
            this.compilation = compilation;
            this.symbol = symbol;
        }

        public string Name
        {
            get { return symbol.Name; }
        }

        public IEnumerable<IAttribute> Attributes
        {
            get
            {
                foreach (var attribute in symbol.GetAttributes())
                {
                    yield return new AttributeWrapper(compilation, attribute);
                }
            }
        }
    }
}