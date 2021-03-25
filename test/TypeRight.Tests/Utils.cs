using System.Linq;

namespace TypeRight.Tests
{
	internal static class Utils
	{

		public static void SplitFullName(string fullName, out string @namespace, out string className)
		{
			string[] split = fullName.Split('.');
			className = split[split.Length - 1];
			@namespace = string.Join(".", split.Take(split.Length - 1));
		}
	}
}
