using System;
using System.Collections.Generic;

namespace Metacoder.Interfaces
{
    public interface IType
    {
        string Name { get; }
        string Namespace { get; }
        string FullName { get; }
        string[] Locations { get; }
        bool IsAbstract { get; }
        bool IsStatic { get; }
        bool Is(Type type);
        bool IsEqualTo(Type type);
        IEnumerable<IMember> Members { get; }
        IAssembly Assembly { get; }
    }
}