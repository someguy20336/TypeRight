using TypeRight.Attributes;
using TypeRight.CodeModel;
using TypeRight.Packages;
using TypeRight.Workspaces.CodeModel;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TypeRight.Workspaces.Parsing
{
	/// <summary>
	/// Parses a single compilation
	/// </summary>
	public class CompilationParser : CSharpSyntaxWalker, ITypeIterator
	{
		private TypeVisitor _visitor;

		/// <summary>
		/// Gets the compilation being parsed
		/// </summary>
		public Compilation Compilation { get; private set; }

		/// <summary>
		/// The parse options
		/// </summary>
		private ParseOptions _options;

		/// <summary>
		/// Creates a new compilation parser
		/// </summary>
		/// <param name="compilation">The compilation</param>
		/// <param name="options">The parse options</param>
		public CompilationParser(Compilation compilation, ParseOptions options)
		{
			Compilation = compilation;
			_options = options;
		}

		/// <summary>
		/// Iterates the types in the compilation
		/// </summary>
		/// <param name="visitor">The visitor for the iterator</param>
		public void IterateTypes(TypeVisitor visitor)
		{
			_visitor = visitor;
			FindExternalTypes();
			foreach (SyntaxTree tree in Compilation.SyntaxTrees)
			{
				Visit(tree.GetRoot());
			}
		}


		/// <summary>
		/// Looks for external types in the current compilation
		/// </summary>
		private void FindExternalTypes()
		{
			
			INamedTypeSymbol exterTypeExtrAttr = Compilation.GetTypeByMetadataName(typeof(ExternalScriptObjectAttribute).FullName);
			foreach (AttributeData attrData in Compilation.Assembly.GetAttributes())
			{
				if (attrData.AttributeClass.Equals(exterTypeExtrAttr))
				{
					TypedConstant typeParamsArg = attrData.ConstructorArguments[0];

					foreach (TypedConstant oneTypeArg in typeParamsArg.Values)
					{
						INamedTypeSymbol type = oneTypeArg.Value as INamedTypeSymbol;

                        ParseContext context = new ParseContext(Compilation, GetDocumentationProvider(type), _options);
                        INamedType namedType = RoslynType.CreateNamedType(type, context); 
						_visitor.VisitExternalType(namedType);
					}
				}
			}
		}

        /// <summary>
        /// Gets the appropriate documentation provider for the type symbol
        /// </summary>
        /// <param name="typeSymbol">the named type symbol</param>
        /// <returns>TThe documentation provider</returns>
        private DocumentationProvider GetDocumentationProvider(INamedTypeSymbol typeSymbol)
        {
            Location location = typeSymbol.Locations[0];
            if (location.Kind == LocationKind.MetadataFile)
            {
                return new ExternalReferenceDocumentationProvider(Compilation);
            }
            else
            {
                return new SourceCommentsDocumentationProvider();
            }
        }
				
		/// <summary>
		/// Visits an Interface declaration
		/// </summary>
		/// <param name="node">the syntax node</param>
		public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
		{
			TryAddTypeDeclaration(node);
		}

		/// <summary>
		/// Visits a class. 
		/// </summary>
		/// <param name="node">The class node</param>
		public override void VisitClassDeclaration(ClassDeclarationSyntax node)
		{
			TryAddTypeDeclaration(node);
		}

		/// <summary>
		/// Trys to add a type declaration node to the extract depending on whether it has
		/// the <see cref="ScriptObjectAttribute"/>
		/// </summary>
		/// <param name="node">The type declaration node</param>
		private void TryAddTypeDeclaration(TypeDeclarationSyntax node)
		{
			INamedTypeSymbol namedType = Compilation.GetSemanticModel(node.SyntaxTree).GetDeclaredSymbol(node);

			RoslynNamedType namedTypeResult = RoslynType.CreateNamedType(
				namedType, 
				new ParseContext(Compilation, new SourceCommentsDocumentationProvider(), _options)
				);
			_visitor.Visit(namedTypeResult);
		}



		/// <summary>
		/// Visits enums and adds script enums to the package
		/// </summary>
		/// <param name="node">The node</param>
		public override void VisitEnumDeclaration(EnumDeclarationSyntax node)
		{
			INamedTypeSymbol namedType = Compilation.GetSemanticModel(node.SyntaxTree).GetDeclaredSymbol(node);
			RoslynNamedType namedTypeResult = RoslynType.CreateNamedType(
				namedType, 
				new ParseContext(Compilation, new SourceCommentsDocumentationProvider(), _options)
				);
			_visitor.Visit(namedTypeResult);
		}


	}
}
