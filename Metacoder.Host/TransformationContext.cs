using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Metacoder.Host.TypeWrappers;
using Metacoder.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.MSBuild;

namespace Metacoder.Host
{
    public class TransformationContext : ITransformationContext
    {
        private MSBuildWorkspace workspace;
        private Project project;
        private Compilation compilation;
        private List<IType> types;
        private List<FileDescription> fileUpdates = new List<FileDescription>();

        public TransformationContext(MSBuildWorkspace workspace, Project project, Compilation compilation)
        {
            this.workspace = workspace;
            this.project = project;
            this.compilation = compilation;
        }

        public IEnumerable<IType> ProjectTypes
        {
            get
            {
                return Types.Where(x => x.Assembly.Name == project.AssemblyName);
            }
        }

        public IEnumerable<IType> Types
        {
            get
            {
                if (types == null)
                {
                    types = new List<IType>();
                    foreach (var syntaxTree in compilation.SyntaxTrees)
                    {
                        var model = compilation.GetSemanticModel(syntaxTree);
                        var typeCollector = new TypeCollector();
                        ((CSharpSyntaxNode)(syntaxTree.GetRoot())).Accept(typeCollector);
                        foreach (var type in typeCollector.DelegateDeclarations)
                        {
                            var symbol = model.GetDeclaredSymbol(type);
                            types.Add(new TypeWrapper(compilation, symbol));
                        }
                        foreach (var type in typeCollector.TypeDeclarations)
                        {
                            var symbol = model.GetDeclaredSymbol(type);
                            types.Add(new TypeWrapper(compilation, symbol));
                        }
                    }                        
                }
                return types;
            }
        }

        public void CreateOrUpdateFile(string location, string content, string dependsOn = null)
        {
            var documentId = project.Solution.GetDocumentIdsWithFilePath(location).Single();
            var document = project.Solution.GetDocument(documentId);
            fileUpdates.Add(new FileDescription
            {
                Project = document.Project,
                Location = location,
                Content = content,
                DependsOn = dependsOn
            });
        }

        public void Finish()
        {
            if (fileUpdates.Any())
            {
                foreach (var filesByProject in fileUpdates.GroupBy(x => x.Project))
                {
                    // Open project file
                    var project = filesByProject.Key;
                    var projectFile = XDocument.Parse(File.ReadAllText(project.FilePath), LoadOptions.PreserveWhitespace);
                    var compileElements = projectFile.Descendants().Where(x => x.Name.LocalName == "Compile").ToDictionary(x => x.Attribute("Include").Value);
                    var compileItemGroup = compileElements.First().Value.Parent;
                    bool modifiedProject = false;

                    foreach (var update in filesByProject)
                    {
                        var name = Path.GetFileName(update.Location);
                        var projectPath = Path.GetDirectoryName(project.FilePath);
                        var relativePath = update.Location.Substring(projectPath.Length + 1);
                        XElement existingElement;
                        if (!compileElements.TryGetValue(name, out existingElement))
                        {
                            modifiedProject = true;
                            var newElement = new XElement(XName.Get("Compile", "http://schemas.microsoft.com/developer/msbuild/2003"));
                            newElement.SetAttributeValue("Include", relativePath);
                            if (update.DependsOn != null)
                            {
                                var dependentUpon = new XElement(XName.Get("DependentUpon", "http://schemas.microsoft.com/developer/msbuild/2003"));
                                dependentUpon.Add(update.DependsOn);
                                newElement.Add(dependentUpon);
                            }

                            compileItemGroup.Add(newElement);
                        }

                        File.WriteAllText(update.Location, update.Content);
                    }

                    if (modifiedProject)
                        File.WriteAllText(project.FilePath, projectFile.ToString(SaveOptions.DisableFormatting));                    
                }
            }
        }
    }
}