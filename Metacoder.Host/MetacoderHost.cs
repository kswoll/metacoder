using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Threading.Tasks;
using Metacoder.Host.Utils;
using Metacoder.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Nito.AsyncEx;

namespace Metacoder.Host
{
    public class MetacoderHost : MarshalByRefObject, IHost
    {
        public void Disconnect()
        {
            RemotingServices.Disconnect(this);
        }

        public void Run(string projectFile)
        {
            AsyncContext.Run(async () =>
            {
                var workspace = Profiler.Time("Creating workspace", () => MSBuildWorkspace.Create());
                var project = await Profiler.Time("Opening project", async () => await workspace.OpenProjectAsync(projectFile));
                var assembly = Assembly.Load(project.AssemblyName);
                var transformers = assembly.GetTypes().Where(x => typeof(IMetacoder).IsAssignableFrom(x)).ToArray();
                var transformationContext = new TransformationContext(workspace, project, await project.GetCompilationAsync());

                Profiler.Time("Running transformers", () =>
                {
                    foreach (var transformerType in transformers)
                    {
                        var transformer = (IMetacoder)Activator.CreateInstance(transformerType);
                        transformer.Transform(transformationContext);
                    }                    
                });

                Profiler.Time("Applying changes", () =>
                {
                    transformationContext.Finish();
                });
            });
        }
    }
}