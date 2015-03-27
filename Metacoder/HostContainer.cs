using System;
using System.IO;
using System.Threading.Tasks;
using Metacoder.Host.Utils;
using Metacoder.Interfaces;

namespace Metacoder
{
    public class HostContainer : IDisposable
    {
        private string projectFile;
        private string targetProjectAssembly;
        private AppDomain appDomain;
        private DirectoryInfo tempFolder;

        public HostContainer(string projectFile, string targetProjectAssembly)
        {
            this.projectFile = projectFile;
            this.targetProjectAssembly = targetProjectAssembly;
            Install();
        }

        private void Install()
        {
            Profiler.Time("Installing...", () =>
            {
                var tempFolderPath = Path.GetRandomFileName();
                var tempRoot = Directory.GetCurrentDirectory();
                var installFolder = new FileInfo(typeof(HostContainer).Assembly.Location).Directory;
                tempFolder = new DirectoryInfo(Path.Combine(tempRoot, tempFolderPath));

                // In the case of a rare collision, re-try the install with a new temp folder
                if (tempFolder.Exists)
                {
                    Install();
                    return;
                }

                tempFolder.Create();

                // Copy all the assemblies of Metacoder into tempFolder
                foreach (var file in installFolder.GetFiles())
                {
                    if (!file.Name.EndsWith(".exe"))
                        file.CopyTo(Path.Combine(tempFolder.FullName, file.Name));
                }

                var targetProjectAssemblyFile = new FileInfo(targetProjectAssembly);
                var targetProjectFolder = targetProjectAssemblyFile.Directory;
                foreach (var file in targetProjectFolder.GetFiles())
                {
                    var destination = new FileInfo(Path.Combine(tempFolder.FullName, file.Name));
                    if (!destination.Exists)
                        file.CopyTo(destination.FullName);
                }

                appDomain = AppDomain.CreateDomain("MetacoderHostContainer", AppDomain.CurrentDomain.Evidence, tempFolder.FullName, ".", true);            
                
            });
        }

        public void Run()
        {
            var host = (IHost)appDomain.CreateInstanceFromAndUnwrap(Path.Combine(tempFolder.FullName, "Metacoder.Host.dll"), "Metacoder.Host.MetacoderHost", null);
            Profiler.Time("Executed host", () => host.Run(projectFile));
        }

        public void Dispose()
        {
            // Unload the host
            AppDomain.Unload(appDomain);

            // Delete the folder that contains the assemblies for the app domain we just unloaded
            foreach (var file in tempFolder.GetFiles())
            {
                file.Delete();
            }
            tempFolder.Delete();
        }
    }
}