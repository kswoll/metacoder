using System.Collections.Generic;
using Metacoder.Interfaces;
using Microsoft.CodeAnalysis;

namespace Metacoder.Host.TypeWrappers
{
    public static class SymbolHelper
    {
        public static IEnumerable<IAttribute> GetAttributes(Compilation compilation, ISymbol symbol)
        {
            foreach (var attribute in symbol.GetAttributes())
            {
                yield return new AttributeWrapper(compilation, attribute);
            }
        }
    }
}