using System.Collections.Generic;

namespace Metacoder.Interfaces
{
    public interface IParameter
    {
        string Name { get; }
        IType Type { get; }
        IEnumerable<IAttribute> Attributes { get; }
    }
}