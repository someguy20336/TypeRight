using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeRightTests.TestBuilders
{
	partial class InterfaceBuilder : IAttributable, IBuilderWithProperties
	{
		private string _interfaceName;
		private string _namespace;

		private List<string> _baseInterfaces = new List<string>();

		private string _comments = "";

		private List<string> _genericParameters = new List<string>();

		private TestProjectBuilder _parentBuilder;

		public List<SymbolInfo> Properties { get; } = new List<SymbolInfo>();

		public List<MethodInfo> Methods { get; set; } = new List<MethodInfo>();

		public List<AttributeInfo> Attributes { get; } = new List<AttributeInfo>();

		public InterfaceBuilder(TestProjectBuilder projBuilder, string interfaceName, string @namespace)
		{
			_parentBuilder = projBuilder;
			_interfaceName = interfaceName;
			_namespace = @namespace;
		}

		public InterfaceBuilder AddGenericParameter(string name)
		{
			_genericParameters.Add(name);
			return this;
		}

		public InterfaceBuilder AddBaseInterface(string baseInterfaceName)
		{
			_baseInterfaces.Add(baseInterfaceName);
			return this;
		}
			   		
		public TestProjectBuilder Commit()
		{
			string text = TransformText();
			SourceText sourceText = SourceText.From(text);
			_parentBuilder.Workspace.AddDocument(_parentBuilder.ProjectID, _interfaceName + ".cs", sourceText);
			return _parentBuilder;
		}

		private string GetBaseInterface()
		{
			return _baseInterfaces.Count == 0 ? "" : $": {string.Join(", ", _baseInterfaces)} ";
		}

		private string GetGenericParams()
		{
			return _genericParameters.Count == 0 ? "" : ($"<{string.Join(", ", _genericParameters)}>");
		}

		private string GetAttributes()
		{
			return this.GetAttributeText();
		}
	}
}
