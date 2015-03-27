using System.Collections.Generic;
using Metacoder.Interfaces;
using Microsoft.CodeAnalysis;

namespace Metacoder.Host.TypeWrappers
{
    public struct FieldWrapper : IField
    {
        private Compilation compilation;
        private IFieldSymbol field;

        public FieldWrapper(Compilation compilation, IFieldSymbol field) 
        {
            this.compilation = compilation;
            this.field = field;
        }

        public string Name
        {
            get { return field.Name; }
        }

        public IEnumerable<IAttribute> Attributes
        {
            get { return SymbolHelper.GetAttributes(compilation, field); }
        }

        public IType Type
        {
            get { return new TypeWrapper(compilation, field.Type); }
        }

        public bool IsConst
        {
            get { return field.IsConst; }
        }

        public bool IsReadOnly
        {
            get { return field.IsReadOnly; }
        }

        public bool IsVolatile
        {
            get { return field.IsVolatile; }
        }

        public bool IsStatic
        {
            get { return field.IsStatic; }
        }

        public object ConstantValue
        {
            get { return field.ConstantValue; }
        }
    }
}