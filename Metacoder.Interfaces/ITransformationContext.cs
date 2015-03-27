using System.Collections.Generic;

namespace Metacoder.Interfaces
{
    public interface ITransformationContext
    {
        IEnumerable<IType> ProjectTypes { get; }
        IEnumerable<IType> Types { get; }
        void CreateOrUpdateFile(string location, string contents, string dependsOn = null);
        void DeriveFile(IType type, string suffix, string content);
    }
}