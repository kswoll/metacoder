using System.Collections.Generic;

namespace Metacoder.Interfaces
{
    public interface IMethod : IMember
    {
        IType ReturnType { get; }
        IEnumerable<IParameter> Parameters { get; }
    }
}