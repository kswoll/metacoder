using System;

namespace Metacoder.Interfaces
{
    public static class ITypeExtensions
    {
        public static bool Is<T>(this IType type)
        {
            return type.Is(typeof(T));
        }
    }
}