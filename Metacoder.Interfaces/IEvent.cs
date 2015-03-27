namespace Metacoder.Interfaces
{
    public interface IEvent : IMember
    {
        IMethod AddMethod { get; }
        IMethod RemoveMethod { get; }
    }
}