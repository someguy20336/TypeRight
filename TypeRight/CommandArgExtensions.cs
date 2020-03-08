using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeRight
{
	internal static class CommandArgExtensions
	{
		public static bool HasSwitch(this string[] args, string name)
			=> args.Any(arg => arg.Equals(name, StringComparison.OrdinalIgnoreCase));
	}
}
