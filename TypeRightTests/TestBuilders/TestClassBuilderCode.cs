using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeRight.Attributes;
using TypeRight.TypeProcessing;

namespace TypeRightTests.TestBuilders
{
	partial class TestClassBuilder : IAttributable, IBuilderWithTypeNameProperties
	{
		private TestProjectBuilder _parentBuilder;

		private string _className;
		private string _namespace;

		private string _comments = "";

		private string _baseClass = "";
		private string _interface = "";


		private List<string> _genericParameters = new List<string>();

		public List<SymbolInfo> Properties { get; } = new List<SymbolInfo>();

		public List<MethodInfo> Methods { get; set; } = new List<MethodInfo>();

		public List<AttributeInfo> Attributes { get; } = new List<AttributeInfo>();

		public TestClassBuilder(TestProjectBuilder projBuilder, string className, string @namespace = "Test")
		{
			_parentBuilder = projBuilder;
			_className = className;
			_namespace = @namespace;
		}

		public TestClassBuilder SetDocumentationComments(string comments)
		{
			_comments = comments;
			return this;
		}

		public TestClassBuilder AddGenericParameter(string name)
		{
			_genericParameters.Add(name);
			return this;
		}

		public TestClassBuilder AddBaseClass(string baseClassName)
		{
			_baseClass = baseClassName;
			return this;
		}

		public TestClassBuilder AddInterface(string interfaceName)
		{
			_interface = interfaceName;
			return this;
		}

		public TestClassBuilder WithControllerBaseClass(bool aspNetCore = true)
		{
			_baseClass = aspNetCore ? MvcConstants.ControllerBaseFullName_AspNetCore : MvcConstants.ControllerBaseFullName_AspNet;
			return this;
		}

		public TestMethodBuilder AddConstructor()
		{
			return AddMethod(_className, "");
		}

		public TestMethodBuilder AddMethod(string name, string type)
		{
			return new TestMethodBuilder(this, name, type);
		}

		public TestProjectBuilder Commit()
		{
			string text = TransformText();
			SourceText sourceText = SourceText.From(text);
			_parentBuilder.Workspace.AddDocument(_parentBuilder.ProjectID, _className + ".cs", sourceText);
			return _parentBuilder;
		}

		private	string GetBaseClass()
		{
			List<string> baseClassesAndInterfaces = new List<string>()
			{
				_baseClass,
				_interface
			};
			baseClassesAndInterfaces = baseClassesAndInterfaces.Where(val => !string.IsNullOrEmpty(val)).ToList();
			return baseClassesAndInterfaces.Count == 0 ? "" : $": {string.Join(", ", baseClassesAndInterfaces)} ";
		}

		private string GetGenericParams()
		{
			return _genericParameters.Count == 0 ? "" : ($"<{string.Join(", ", _genericParameters)}>");
		}

		private string GetAttributes()
		{
			return this.GetAttributeText();
		}

		private string FormatParameter(SymbolInfo parameter)
		{
			string attrs = "";
			if (parameter.Attributes.Count > 0)
			{
				attrs = $"[{ string.Join(", ", parameter.Attributes.Select(attr => attr.AttributeTypeName)) }]";
			}
			return $"{ attrs }{parameter.Type} {parameter.Name}";
		}
	}
}
