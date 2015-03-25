namespace Metacoder.Interfaces
{
    public interface IField : IMember
    {
        IType Type { get; }
    }
}