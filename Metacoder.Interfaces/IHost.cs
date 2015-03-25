using System.Threading.Tasks;

namespace Metacoder.Interfaces
{
    public interface IHost
    {
        void Run(string projectFile);
    }
}