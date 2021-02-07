using System.Collections.Generic;

namespace TypeRight.Tests.TestBuilders
{
	public class MethodInfo : SymbolInfo
	{
		public List<SymbolInfo> Parameters { get; set; }

		public List<string> LinesOfCode { get; set; }

	}
}
