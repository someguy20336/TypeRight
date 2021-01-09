using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting.TypeScript
{
	/// <summary>
	/// Manages imports for a given typescript file
	/// </summary>
	public class ImportManager
	{
		private string _outputPath;
		private Dictionary<string, ImportStatement> _imports = new Dictionary<string, ImportStatement>();

		public ImportManager(string outputPath)
		{
			_outputPath = outputPath;
		}

		public static ImportManager FromTypes(IEnumerable<ExtractedType> types, string outputPath)
		{
			ImportManager newManager = new ImportManager(outputPath);
			foreach (var type in types.GetReferenceTypes())
			{
				// Check the base type of the type
				if (type.BaseType != null)
				{
					newManager.TryAddToImports(type.BaseType);
				}

				// Check each property type
				foreach (var property in type.Properties)
				{
					newManager.TryAddToImports(property.Type);
				}
			}

			return newManager;
		}

		/// <summary>
		/// Blindly addes the import statement without verifing the output path or anything
		/// </summary>
		/// <param name="path"></param>
		/// <param name="import"></param>
		public void AddImport(string path, ImportStatement import)
		{
			_imports.Add(path, import);
		}

		/// <summary>
		/// Trys to add an import for the given type descriptor if the target path of that type is different than the output path of this file
		/// </summary>
		/// <param name="descriptor"></param>
		public void TryAddToImports(TypeDescriptor descriptor)
		{
			if (descriptor is ExtractedTypeDescriptor extractedType && extractedType.TargetPath != _outputPath)
			{
				if (!_imports.ContainsKey(extractedType.TargetPath))
				{
					_imports.Add(extractedType.TargetPath, new ImportStatement(_outputPath, extractedType.TargetPath, true));
				}
				_imports[extractedType.TargetPath].AddItem(extractedType.Name);

				if (extractedType is NamedReferenceTypeDescriptor refType && refType.TypeArguments.Count > 0)
				{
					foreach (var arg in refType.TypeArguments)
					{
						TryAddToImports(arg);
					}
				}
			}
			else if (descriptor is ListTypeDescriptor listType)
			{
				TryAddToImports(listType.TypeArg);
			}
			else if (descriptor is ArrayTypeDescriptor arrayType)
			{
				TryAddToImports(arrayType.ElementType);
			}
			else if (descriptor is DictionaryTypeDescriptor dictType)
			{
				TryAddToImports(dictType.Value);
			}
			else if (descriptor is NullableTypeDescriptor nullable)
			{
				TryAddToImports(nullable.TypeArgument);
			}
			else if (descriptor is AnonymousTypeDescriptor anonymous)
			{
				foreach (ExtractedProperty property in anonymous.Properties)
				{
					TryAddToImports(property.Type);
				}
			}
		}

		public IEnumerable<ImportStatement> GetImports() => _imports.Values;

		public bool ContainsImportPath(string path) => _imports.ContainsKey(path);

		public ImportStatement GetImportAtPath(string path) => _imports[path];
	}
}
