﻿using System.Collections.Generic;
using TypeRight.Configuration;
using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting.TypeScript
{
	public enum ImportModuleNameStyle
	{
		Extensionless,
		ReplaceWithJs
	}

	/// <summary>
	/// Manages imports for a given typescript file
	/// </summary>
	public class ImportManager
	{
		private readonly string _outputPath;
		private readonly Dictionary<string, ImportStatement> _imports = new Dictionary<string, ImportStatement>();
		private readonly ImportModuleNameStyle _namingStyle;

		private ImportManager(string outputPath, ImportModuleNameStyle nameStyle)
		{
			_outputPath = outputPath;
			_namingStyle = nameStyle;
		}

		public static ImportManager FromTypes(IEnumerable<ExtractedType> types, string outputPath, ImportModuleNameStyle nameStyle)
		{
			ImportManager newManager = new ImportManager(outputPath, nameStyle);
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

		public static ImportManager FromControllerContext(ControllerContext context, ImportModuleNameStyle nameStyle)
		{
			ImportManager result = new ImportManager(context.OutputPath, nameStyle);
			foreach (MvcAction actionInfo in context.Actions)
			{
				CompileActionImport(result, context, actionInfo);
			}

			return result;
		}


		private static void CompileActionImport(ImportManager imports, ControllerContext context, MvcAction actionInfo)
		{
			FetchFunctionDescriptor fetchDescriptor = context.FetchFunctionResolver.Resolve(actionInfo.RequestMethod.Name);

			string funcKey = "fetch-" + fetchDescriptor.FetchModulePath;
			if (!imports.ContainsImportPath(funcKey))
			{
				imports.AddImport(funcKey, context.OutputPath, fetchDescriptor.FetchModulePath, false);
			}
			ImportStatement ajaxImport = imports.GetImportAtPath(funcKey);
			ajaxImport.AddItem(fetchDescriptor.FunctionName);

			AddActionImports(imports, actionInfo);
			TryAddAdditionalImports(imports, context, fetchDescriptor.AdditionalImports);
		}


		private static void AddActionImports(ImportManager imports, MvcAction action)
		{
			imports.TryAddToImports(action.ReturnType);
			foreach (var param in action.ActionParameters)
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
					imports.AddImport(key, context.OutputPath, importPath, def.UseAlias);
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

		private void AddImport(string key, string outputPath, string fromPath, bool useAlias)
		{
			_imports.Add(key, new ImportStatement(outputPath, fromPath, useAlias, _namingStyle));
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
					AddImport(extractedType.TargetPath, _outputPath, extractedType.TargetPath, true);
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
