using System.Collections.Generic;

namespace TypeRight.Tests.TestBuilders
{
	public class EnumMemberInfo
	{
		public string Comments { get; set; }

		public string Name { get; set; }

		public string Value { get; set; }

		public List<AttributeInfo> Attributes { get; set; }
	}
}
