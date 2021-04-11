using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TypeRight.Configuration;
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

		private ImportManager(string outputPath)
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

		public static ImportManager FromController(ControllerContext context)
		{
			ImportManager result = new ImportManager(context.OutputPath);
			foreach (MvcActionInfo actionInfo in context.Controller.Actions)
			{
				CompileActionImport(result, context, actionInfo);
			}

			return result;
		}


		private static void CompileActionImport(ImportManager imports, ControllerContext context, MvcActionInfo actionInfo)
		{
			FetchFunctionDescriptor fetchDescriptor = context.FetchFunctionResolver.Resolve(actionInfo.RequestMethod.Name);

			string funcKey = "fetch-" + fetchDescriptor.FetchModulePath;
			if (!imports.ContainsImportPath(funcKey))
			{
				imports.AddImport(funcKey, new ImportStatement(context.OutputPath, fetchDescriptor.FetchModulePath, false));
			}
			ImportStatement ajaxImport = imports.GetImportAtPath(funcKey);
			ajaxImport.AddItem(fetchDescriptor.FunctionName);

			AddActionImports(imports, actionInfo);
			TryAddAdditionalImports(imports, context, fetchDescriptor.AdditionalImports);
		}


		private static void AddActionImports(ImportManager imports, MvcActionInfo action)
		{
			imports.TryAddToImports(action.ReturnType);
			foreach (var param in action.Parameters)
			{
				foreach (var type in param.Types)
				{
					imports.TryAddToImports(type);
				}
			}
		}

		private static void TryAddAdditionalImports(ImportManager imports, ControllerContext context, IEnumerable<ImportDefinition> additionalImports)
		{
			// Additional imports
			foreach (ImportDefinition def in additionalImports)
			{
				string importPath = PathUtils.ResolveRelativePath(context.OutputPath, def.Path);

				string key = "custom" + importPath;
				if (!imports.ContainsImportPath(key))
				{
					imports.AddImport(key, new ImportStatement(context.OutputPath, importPath, def.UseAlias));
				}

				ImportStatement statement = imports.GetImportAtPath(key);
				if (def.Items != null)
				{
					foreach (var item in def.Items)
					{
						statement.AddItem(item);
					}
				}

			}
		}


		public IEnumerable<ImportStatement> GetImports() => _imports.Values;

		public bool ContainsImportPath(string path) => _imports.ContainsKey(path);

		public ImportStatement GetImportAtPath(string path) => _imports[path];


		/// <summary>
		/// Blindly addes the import statement without verifing the output path or anything
		/// </summary>
		/// <param name="path"></param>
		/// <param name="import"></param>
		private void AddImport(string path, ImportStatement import)
		{
			_imports.Add(path, import);
		}

		/// <summary>
		/// Trys to add an import for the given type descriptor if the target path of that type is different than the output path of this file
		/// </summary>
		/// <param name="descriptor"></param>
		private void TryAddToImports(TypeDescriptor descriptor)
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
	}
}
