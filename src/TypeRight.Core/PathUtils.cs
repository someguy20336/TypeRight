using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeRight
{
	/// <summary>
	/// Utilities for working with paths
	/// </summary>
	internal static class PathUtils
	{
		public static string MakeRelativePath(string from, string to)
		{

			Uri fromUri = new Uri(from);
			Uri toUri = new Uri(to);

			return fromUri.MakeRelativeUri(toUri).ToString();
		}

		public static string ResolveRelativePath(string basePath, string relative)
		{
			Uri baseUri = new Uri(basePath);
			Uri relativeUri = new Uri(relative, UriKind.RelativeOrAbsolute);
			Uri resolved = new Uri(baseUri, relativeUri);
			return resolved.LocalPath;
		}
	}
}
