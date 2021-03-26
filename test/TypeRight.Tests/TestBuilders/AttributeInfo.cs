using System.Collections.Generic;
using System.Linq;

namespace TypeRight.Tests.TestBuilders
{
	public class AttributeInfo
	{
		public string AttributeTypeName { get; set; }

		public List<string> CtorArguments { get; set; } = new List<string>();

		public Dictionary<string, string> NamedArguments { get; set; } = new Dictionary<string, string>();

		public string ToFormattedString()
		{
			List<string> allArgs = new List<string>(CtorArguments);
			allArgs.AddRange(NamedArguments.Select(nm => $"{nm.Key} = {nm.Value}"));
			return $"{AttributeTypeName}({string.Join(", ", allArgs)})";
		}
	}
}
