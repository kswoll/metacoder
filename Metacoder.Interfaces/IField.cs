namespace Metacoder.Interfaces
{
    public interface IField : IMember
    {
        IType Type { get; }
        bool IsConst { get; }
        bool IsReadOnly { get; }
        bool IsVolatile { get; }
        bool IsStatic { get; }
        object ConstantValue { get; }
    }
}