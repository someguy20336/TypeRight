using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeRightTests.TestBuilders
{
	class AttributeInfo
	{
		public string AttributeTypeName { get; set; }

		public List<string> CtorArguments { get; set; }

		public Dictionary<string, string> NamedArguments { get; set; }

		public string ToFormattedString()
		{
			List<string> allArgs = new List<string>(CtorArguments);
			allArgs.AddRange(NamedArguments.Select(nm => $"{nm.Key} = {nm.Value}"));
			return $"{AttributeTypeName}({string.Join(", ", allArgs)})";
		}
	}
}
