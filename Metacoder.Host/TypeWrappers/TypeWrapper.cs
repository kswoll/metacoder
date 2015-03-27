using System;
using System.Collections.Generic;
using System.Linq;
using Metacoder.Host.Utils;
using Metacoder.Interfaces;
using Microsoft.CodeAnalysis;

namespace Metacoder.Host.TypeWrappers
{
    public struct TypeWrapper : IType, IMember
    {
        private Compilation compilation;
        private ITypeSymbol symbol;

        public TypeWrapper(Compilation compilation, ITypeSymbol symbol)
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
            get { return SymbolHelper.GetAttributes(compilation, symbol); }
        }

        public string Namespace
        {
            get { return symbol.ContainingNamespace.GetFullName(); }
        }

        public string FullName
        {
            get { return symbol.GetFullName(); }
        }

        public string[] Locations
        {
            get { return symbol.Locations.Where(x => x.SourceTree != null).Select(x => x.SourceTree.FilePath).ToArray(); }
        }

        public bool IsAbstract
        {
            get { return symbol.IsAbstract; }
        }

        public bool IsStatic
        {
            get { return symbol.IsStatic; }
        }

        public bool Is(Type type)
        {
            var symbol = compilation.GetTypeByMetadataName(type.FullName);
            return symbol.IsAssignableFrom(this.symbol);
        }

        public bool IsEqualTo(Type type)
        {
            return symbol.GetFullName() == type.FullName;
        }

        public IEnumerable<IMember> Members
        {
            get
            {
                foreach (var member in symbol.GetMembers())
                {
                    if (member is IFieldSymbol)
                    {
                        var field = (IFieldSymbol)member;
                        var wrapper = new FieldWrapper(compilation, field);
                        yield return wrapper;
                    }
                    else if (member is IMethodSymbol)
                    {
                        var method = (IMethodSymbol)member;
                        var wrapper = new MethodWrapper(compilation, method);
                        yield return wrapper;
                    }
                    else if (member is IPropertySymbol)
                    {
                        var property = (IPropertySymbol)member;
                        var wrapper = new PropertyWrapper(compilation, property);
                        yield return wrapper;
                    }
                    else if (member is IEventSymbol)
                    {
                        var @event = (IEventSymbol)member;
                        var wrapper = new EventWrapper(compilation, @event);
                        yield return wrapper;
                    }
                    else if (member is ITypeSymbol)
                    {
                        var type = (ITypeSymbol)member;
                        var wrapper = new TypeWrapper(compilation, type);
                        yield return wrapper;
                    }
                }
            }
        }

        public IAssembly Assembly
        {
            get { return new AssemblyWrapper(symbol.ContainingAssembly); }
        }
    }
}