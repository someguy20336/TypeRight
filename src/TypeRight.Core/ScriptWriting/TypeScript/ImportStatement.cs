using System;
using System.Collections.Generic;

namespace TypeRight.ScriptWriting.TypeScript
{
    /// <summary>
    /// A typescript import statement
    /// </summary>
    public class ImportStatement
	{
		private readonly HashSet<string> _items = new HashSet<string>();

		/// <summary>
		/// Gets the imported items
		/// </summary>
		public IEnumerable<string> Items => _items;

		/// <summary>
		/// Gets the relative path for the import
		/// </summary>
		public string FromRelativePath { get; }

		/// <summary>
		/// Gets whether the asterisk notation should be used (i.e. * as thing)
		/// </summary>
		public bool UseAsteriskNotation { get; }

		/// <summary>
		/// Gets the import alias for the asterist notation
		/// </summary>
		public string ImportAlias { get; }

		/// <summary>
		/// Creates a new import statement
		/// </summary>
		/// <param name="basePath"></param>
		/// <param name="importPath"></param>
		/// <param name="useAsteriskNotation"></param>
		public ImportStatement(string basePath, string importPath, bool useAsteriskNotation, ImportModuleNameStyle nameStyle)
		{
			UseAsteriskNotation = useAsteriskNotation;
			Uri fromUri = new Uri(basePath);
			Uri toUri = new Uri(importPath);
			
			FromRelativePath = fromUri.MakeRelativeUri(toUri).ToString();

			// Make sure it has a ./ before anything in the same directory
			if (!FromRelativePath.StartsWith("."))
			{
				FromRelativePath = "./" + FromRelativePath;
			}
			FromRelativePath = AdjustImportModuleName(nameStyle);

			// Import alias
			if (UseAsteriskNotation)
			{
				string[] parts = FromRelativePath.Split('/');
				ImportAlias = parts[parts.Length - 1].Replace(".js", "");	// remove any .js for a nice name
			}
		}

		private string AdjustImportModuleName(ImportModuleNameStyle nameStyle)
        {
            switch (nameStyle)
            {
                case ImportModuleNameStyle.ReplaceWithJs:
					return HandleReplaceWithJs(FromRelativePath);
                default:
                    return HandleExtensionless(FromRelativePath);
            }
        }

		private static string HandleExtensionless(string path)
        {
            if (path.EndsWith(".ts", StringComparison.OrdinalIgnoreCase))
            {
				return path.Substring(0, path.Length - 3);
			}

			return path;
        }

		private static string HandleReplaceWithJs(string path)
		{
			if (path.EndsWith(".ts", StringComparison.OrdinalIgnoreCase))
			{
				return path.Substring(0, path.Length - 3) + ".js";
			}

			return path;
		}

		/// <summary>
		/// Adds an item to the import
		/// </summary>
		/// <param name="itemName">the name of the item</param>
		public void AddItem(string itemName)
		{
			if (_items.Contains(itemName))
			{
				return;
			}
			_items.Add(itemName);
		}

		/// <summary>
		/// Gets the string import statement
		/// </summary>
		/// <returns></returns>
		public string ToImportStatement()
		{
			string importCore;
			if (UseAsteriskNotation)
			{
				importCore = $"* as { ImportAlias }";
			}
			else
			{
				importCore = "{ " + string.Join(", ", _items) + " }";
			}
			return $"import {importCore} from \"{FromRelativePath}\";";
		}
	}
}
