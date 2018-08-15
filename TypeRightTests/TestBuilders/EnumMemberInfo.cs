using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeRightTests.TestBuilders
{
	class EnumMemberInfo
	{
		public string Comments { get; set; }

		public string Name { get; set; }

		public string Value { get; set; }

		public List<AttributeInfo> Attributes { get; set; }
	}
}
