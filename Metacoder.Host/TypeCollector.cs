using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Metacoder.Host
{
    public class TypeCollector : CSharpSyntaxWalker
    {
        private List<BaseTypeDeclarationSyntax> typeDeclarations = new List<BaseTypeDeclarationSyntax>();
        private List<DelegateDeclarationSyntax> delegateDeclarations = new List<DelegateDeclarationSyntax>();

        public List<BaseTypeDeclarationSyntax> TypeDeclarations
        {
            get { return typeDeclarations; }
        }

        public List<DelegateDeclarationSyntax> DelegateDeclarations
        {
            get { return delegateDeclarations; }
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            typeDeclarations.Add(node);
            base.VisitClassDeclaration(node);
        }

        public override void VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            typeDeclarations.Add(node);
            base.VisitEnumDeclaration(node);
        }

        public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            typeDeclarations.Add(node);
            base.VisitInterfaceDeclaration(node);
        }

        public override void VisitDelegateDeclaration(DelegateDeclarationSyntax node)
        {
            delegateDeclarations.Add(node);
            base.VisitDelegateDeclaration(node);
        }

        public override void VisitStructDeclaration(StructDeclarationSyntax node)
        {
            typeDeclarations.Add(node);
            base.VisitStructDeclaration(node);
        }
    }
}