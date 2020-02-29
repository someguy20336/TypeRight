using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeRight.Tests.TestBuilders
{
	class MethodInfo : SymbolInfo
	{
		public List<SymbolInfo> Parameters { get; set; }

		public List<string> LinesOfCode { get; set; }

	}
}
