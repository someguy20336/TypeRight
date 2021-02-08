using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Text;

namespace TypeRight.Tests.TestBuilders
{
	public class AssemblyAttributeBuilder : IAttributable
	{
		private string _fileName;
		private TestProjectBuilder _parentBuilder;

		public AssemblyAttributeBuilder(TestProjectBuilder parentBuilder, string fileName)
		{
			_parentBuilder = parentBuilder;
			_fileName = fileName;
		}

		public List<AttributeInfo> Attributes { get; } = new List<AttributeInfo>();

		public TestAttributeBuilder<AssemblyAttributeBuilder> AddAttribute(string attrName)
		{
			return new TestAttributeBuilder<AssemblyAttributeBuilder>(this, attrName);
		}

		public TestProjectBuilder Commit()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (AttributeInfo attributeInfo in Attributes)
			{
				stringBuilder.AppendLine($"[assembly:{attributeInfo.AttributeTypeName}({string.Join(", ", attributeInfo.CtorArguments)})]");
			}

			SourceText sourceText = SourceText.From(stringBuilder.ToString());
			_parentBuilder.Workspace.AddDocument(_parentBuilder.ProjectID, _fileName + ".cs", sourceText);
			return _parentBuilder;
		}
	}
}
