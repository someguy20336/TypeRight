using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeRightTests.TestBuilders
{
	class TestEnumMemberBuilder : IAttributable
	{
		private TestEnumBuilder _parent;

		private string _name = "";

		private string _value = "";

		private string _comments = "";

		public List<AttributeInfo> Attributes { get; } = new List<AttributeInfo>();

		public TestEnumMemberBuilder(TestEnumBuilder parentClass, string name, string value, string comments)
		{
			_parent = parentClass;
			_name = name;
			_value = value;
			_comments = comments;
		}

		public TestAttributeBuilder<TestEnumMemberBuilder> AddAttribute(string attributeType)
		{
			return new TestAttributeBuilder<TestEnumMemberBuilder>(this, attributeType);
		}

		public TestEnumBuilder Commit()
		{
			EnumMemberInfo method = new EnumMemberInfo()
			{
				Name = _name,
				Comments = _comments,
				Value = _value,
				Attributes = Attributes
			};
			_parent.Members.Add(method);
			return _parent;
		}
	}
}
