using System.Collections.Generic;
using Metacoder.Interfaces;
using Microsoft.CodeAnalysis;

namespace Metacoder.Host.TypeWrappers
{
    public class FieldWrapper : SymbolWrapper, IField
    {
        private IFieldSymbol field;

        public FieldWrapper(Compilation compilation, IFieldSymbol field) : base(compilation, field)
        {
            this.field = field;
        }

        public IType Type
        {
            get { return new TypeWrapper(compilation, field.Type); }
        }
    }
}