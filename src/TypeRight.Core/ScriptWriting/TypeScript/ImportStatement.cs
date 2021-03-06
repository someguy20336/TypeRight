﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeRight.ScriptWriting.TypeScript
{
	/// <summary>
	/// A typescript import statement
	/// </summary>
	public class ImportStatement
	{
		private HashSet<string> _items = new HashSet<string>();

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
		public ImportStatement(string basePath, string importPath, bool useAsteriskNotation)
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
			FromRelativePath = FromRelativePath.Substring(0, FromRelativePath.Length - 3);  // remove .ts

			// Import alias
			if (UseAsteriskNotation)
			{
				string[] parts = FromRelativePath.Split('/');
				ImportAlias = parts[parts.Length - 1];
			}
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
