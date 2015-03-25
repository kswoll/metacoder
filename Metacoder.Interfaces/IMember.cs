using System.Collections.Generic;

namespace Metacoder.Interfaces
{
    public interface IMember
    {
        string Name { get; }
        IEnumerable<IAttribute> Attributes { get; }
    }
}