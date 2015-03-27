using System.Collections.Generic;

namespace Metacoder.Interfaces
{
    public interface IAttribute
    {
        IType Type { get; }
        IMethod Constructor { get; }
        object[] ConstructorArguments { get; }
        Dictionary<string, object> NamedArguments { get; }
    }
}