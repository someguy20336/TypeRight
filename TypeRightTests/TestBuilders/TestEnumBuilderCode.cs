using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeRightTests.TestBuilders
{
	partial class TestEnumBuilder : IAttributable
	{

		private TestProjectBuilder _parentBuilder;
		private string _enumName = "";

		private string _comments = "";

		public List<EnumMemberInfo> Members { get; set; } = new List<EnumMemberInfo>();

		public List<AttributeInfo> Attributes { get; } = new List<AttributeInfo>();

		public TestEnumBuilder(TestProjectBuilder projBuilder, string enumName)
		{
			_parentBuilder = projBuilder;
			_enumName = enumName;
		}

		public TestEnumBuilder AddComments(string comments)
		{
			_comments = comments;
			return this;
		}

		public TestEnumMemberBuilder AddMember(string name, string value, string comments = "")
		{
			return new TestEnumMemberBuilder(this, name, value, comments);
		}

		public TestProjectBuilder Commit()
		{
			string text = TransformText();
			SourceText sourceText = SourceText.From(text);
			_parentBuilder.Workspace.AddDocument(_parentBuilder.ProjectID, _enumName + ".cs", sourceText);
			return _parentBuilder;
		}
	}
}
