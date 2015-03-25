using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Metacoder.Host.Utils
{
    public static class RoslynExtensions
    {
        public static bool HasAttribute(this ISymbol symbol, INamedTypeSymbol attributeType)
        {
            var jsAttributes = symbol.GetAttributes().Where(x => Equals(x.AttributeClass, attributeType)).ToArray();
            return jsAttributes.Any();
        }

        public static T GetAttributeValue<T>(this ISymbol symbol, INamedTypeSymbol attributeType, string propertyName, T defaultValue = default(T))
        {
            var jsAttributes = symbol.GetAttributes().Where(x => Equals(x.AttributeClass, attributeType)).ToArray();
            foreach (var jsAttribute in jsAttributes)
            {
                var argument = jsAttribute.NamedArguments.SingleOrDefault(x => x.Key == propertyName);
                if (argument.Value.Kind == TypedConstantKind.Array)
                {
                    var elementType = typeof(T).GetElementType();
                    var source = argument.Value.Values.Select(x => x.Value).ToArray();
                    var array = Array.CreateInstance(elementType, source.Length);
                    source.CopyTo(array, 0);
                    return (T)(object)array;
                }
                else
                {
                    if (argument.Value.Value != null)
                    {
                        return (T)argument.Value.Value;
                    }                    
                }
            }
            return defaultValue;
        }

        public static string GetFullName(this ITypeSymbol type)
        {
            if (type.IsAnonymousType)
            {
                return type.GetTypeName();
            }
            if (type is IArrayTypeSymbol)
            {
                var arrayType = (IArrayTypeSymbol)type;
                return arrayType.ElementType.GetFullName() + "[]";
            }

            var typeParameter = type as ITypeParameterSymbol;
            if (typeParameter != null)
            {
                return typeParameter.Name;
            }
            else
            {
                string result = type.MetadataName;
                if (type.ContainingType != null)
                    result = type.ContainingType.GetFullName() + "." + result;
                else if (!type.ContainingNamespace.IsGlobalNamespace)
                    result = type.ContainingNamespace.GetFullName() + "." + result;
                return result;
            }
        }

        private static int anonymousTypeNameCounter = 1;
        private static Dictionary<ITypeSymbol, string> anonymousTypeNames = new Dictionary<ITypeSymbol, string>();

        public static string GetTypeName(this ITypeSymbol type)
        {
            if (type.IsAnonymousType)
            {
                string name;
                if (!anonymousTypeNames.TryGetValue(type, out name))
                {
                    var index = anonymousTypeNameCounter++;
                    name = type.ContainingAssembly.GetAssemblyAnonymousTypesArray() + "[" + index + "]";
                    anonymousTypeNames[type] = name;
                }
                return name;
            }

            var namedTypeSymbol = type as INamedTypeSymbol;
            if (namedTypeSymbol != null)
            {
                var result = type.GetFullName().Replace('`', '$');
                return result;
            }
            else if (type is IArrayTypeSymbol)
            {
                var arrayType = (IArrayTypeSymbol)type;
                return GetTypeName(arrayType.ElementType) + "$1";
            }
            else if (type is ITypeParameterSymbol)
            {
                var typeParameter = (ITypeParameterSymbol)type;
                return typeParameter.Name;
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetFullName(this INamespaceSymbol namespaceSymbol)
        {
            string result = namespaceSymbol.MetadataName;
            if (!namespaceSymbol.IsGlobalNamespace && !namespaceSymbol.ContainingNamespace.IsGlobalNamespace)
                result = namespaceSymbol.ContainingNamespace.GetFullName() + "." + result;
            return result;
        }

        public static string GetAssemblyAnonymousTypesArray(this IAssemblySymbol assembly)
        {
            return "$" + assembly.Name.MaskSpecialCharacters() + "$AnonymousTypes";
        }

        public static string MaskSpecialCharacters(this string s)
        {
            return s.Replace('.', '$').Replace('<', '$').Replace('>', '$').Replace(',', '$');
        }

        public static bool IsAssignableFrom(this ITypeSymbol baseType, ITypeSymbol type)
        {
            var current = type;
            while (current != null)
            {
                if (Equals(current, baseType))
                    return true;
                current = current.BaseType;
            }
            foreach (var intf in type.AllInterfaces)
            {
                if (Equals(intf, baseType))
                    return true;
            }
            return false;
        }
    }
}