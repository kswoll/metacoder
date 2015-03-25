using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace Metacoder
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide two arguments:");
                Console.WriteLine("1. The fully qualified path to your .csproj file.");
                Console.WriteLine("2. The fully qualified path to your project's assembly.");
                Console.WriteLine();
                Console.WriteLine("For example:");
                Console.WriteLine(@"Metacoder.exe c:\dev\MyProject\MyProject.csproj c:\dev\MyProject\bin\MyProject.dll");
                return;
            }

            using (var hostContainer = new HostContainer(args[0], args[1]))
            {
                hostContainer.Run();
            };
        }
    }
}
