using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeRight.Attributes;

namespace TypeRight.Tests.TestBuilders
{
	public class TestMethodBuilder : IAttributable
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

		public TestMethodBuilder AddParameter(string paramName, string type, string comments = "", string attribute = "")
		{
			var attrs = string.IsNullOrEmpty(attribute)
				? new List<AttributeInfo>()
				: new List<AttributeInfo>() { new AttributeInfo() { AttributeTypeName = attribute } };
			
			return AddParameter(paramName, type, comments, attrs);
		}

		public TestMethodBuilder AddParameter(string paramName, string type, string comments, List<AttributeInfo> attributes)
		{
			_parameters.Add(new SymbolInfo()
			{
				Name = paramName,
				Type = type,
				Comments = comments,
				Attributes = attributes ?? new List<AttributeInfo>()
			});
			return this;
		}

		public TestAttributeBuilder<TestMethodBuilder> AddAttribute(string attributeType)
		{
			return new TestAttributeBuilder<TestMethodBuilder>(this, attributeType);
		}

		public TestMethodBuilder AddScriptActionAttribute()
		{
			AddAttribute(typeof(ScriptActionAttribute).FullName).Commit();
			return this;
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
