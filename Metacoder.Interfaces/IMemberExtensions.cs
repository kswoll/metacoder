using System;
using System.Linq;

namespace Metacoder.Interfaces
{
    public static class IMemberExtensions
    {
        public static bool HasAttribute(this IMember member, Type attributeType)
        {
            return member.Attributes.Any(x => x.Type.IsEqualTo(attributeType));
        }

        public static bool HasAttribute<T>(this IMember member)
        {
            return member.HasAttribute(typeof(T));
        }
    }
}