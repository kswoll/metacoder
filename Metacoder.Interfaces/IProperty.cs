using System.Collections.Generic;

namespace Metacoder.Interfaces
{
    public interface IProperty : IMember
    {
        IType Type { get; }
        bool IsIndexer { get; }
        IMethod Getter { get; }
        IMethod Setter { get; }
        IEnumerable<IParameter> Parameters { get; }
    }
}