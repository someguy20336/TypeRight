using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeRightTests.TestBuilders
{
	class TestMethodBuilder : IAttributable
	{
		private TestClassBuilder _parent;
		private string _returnType = "";

		private string _name = "";

		private string _comments = "";

		private List<SymbolInfo> _parameters = new List<SymbolInfo>();

		private List<string> _linesOfCode = new List<string>();

		public List<AttributeInfo> Attributes { get; } = new List<AttributeInfo>();

		public TestMethodBuilder(TestClassBuilder parentClass, string name, string returnType)
		{
			_parent = parentClass;
			_name = name;
			_returnType = returnType;
		}

		public TestMethodBuilder AddParameter(string paramName, string type, string comments = "")
		{
			_parameters.Add(new SymbolInfo()
			{
				Name = paramName,
				Type = type,
				Comments = comments
			});
			return this;
		}

		public TestAttributeBuilder<TestMethodBuilder> AddAttribute(string attributeType)
		{
			return new TestAttributeBuilder<TestMethodBuilder>(this, attributeType);
		}

		public TestMethodBuilder AddLineOfCode(string code, int indent)
		{
			string tabs = new string('\t', indent);
			_linesOfCode.Add(tabs + code);
			return this;
		}

		public TestClassBuilder Commit()
		{
			MethodInfo method = new MethodInfo()
			{
				Name = _name,
				Type = _returnType,
				Comments = _comments,
				Parameters = _parameters,
				LinesOfCode = _linesOfCode,
				Attributes = Attributes
			};
			_parent.Methods.Add(method);
			return _parent;
		}
	}
}
