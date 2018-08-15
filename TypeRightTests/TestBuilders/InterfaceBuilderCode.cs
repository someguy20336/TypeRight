using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeRightTests.TestBuilders
{
	partial class InterfaceBuilder 
	{
		private string _interfaceName;

		private List<string> _baseInterfaces = new List<string>();

		private string _comments = "";

		private List<string> _genericParameters = new List<string>();

		private List<SymbolInfo> _properties = new List<SymbolInfo>();

		private TestProjectBuilder _parentBuilder;

		public List<MethodInfo> Methods { get; set; } = new List<MethodInfo>();

		public InterfaceBuilder(TestProjectBuilder projBuilder, string interfaceName)
		{
			_parentBuilder = projBuilder;
			_interfaceName = interfaceName;
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


		public InterfaceBuilder AddProperty(string name, string type, string comments = "")
		{
			_properties.Add(new SymbolInfo() { Name = name, Type = type, Comments = comments });
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
	}
}
