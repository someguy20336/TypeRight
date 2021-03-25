using System.Collections.Generic;

namespace TypeRight.Tests.TestBuilders
{
	public class SymbolInfo : IAttributable
	{
		public string Name { get; set; }

		public string Type { get; set; }

		public string Comments { get; set; }

		public List<AttributeInfo> Attributes { get; set; } = new List<AttributeInfo>();

	}


}
