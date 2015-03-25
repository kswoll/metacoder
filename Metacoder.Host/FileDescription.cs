using Microsoft.CodeAnalysis;

namespace Metacoder.Host
{
    public class FileDescription
    {
        public Project Project { get; set; }
        public string Location { get; set; }
        public string DependsOn { get; set; }
        public string Content { get; set; }
    }
}