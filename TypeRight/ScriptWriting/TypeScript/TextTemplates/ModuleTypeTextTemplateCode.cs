using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting.TypeScript.TextTemplates
{
	internal partial class ModuleTypeTextTemplate : ITypeTextTemplate
	{
		private Dictionary<string, ImportStatement> _imports = new Dictionary<string, ImportStatement>();
		private TypeFormatter _formatter;

		private TypeWriteContext _context;
		/// <summary>
		/// Gets the script for the extracted types
		/// </summary>
		/// <param name="context">The context for writing the script</param>
		/// <returns>The script text</returns>
		public string GetText(TypeWriteContext context)
		{
			_context = context;
			
			CompileImports();
			_formatter = new TypeScriptTypeFormatter(context.TypeCollection, new ModuleTypePrefixResolver(_imports));  // Add no prefix to the types

			return TransformText();
		}

		private IEnumerable<ImportStatement> GetImports() => _imports.Values;

		private IEnumerable<ExtractedReferenceType> GetReferenceTypes() => _context.IncludedTypes.GetReferenceTypes().OrderBy(type => type.Name);

		private IEnumerable<ExtractedEnumType> GetEnumTypes() => _context.IncludedTypes.GetEnumTypes().OrderBy(type => type.Name);

		private IPartialTypeTextTemplate GetPartialTextTemplate(ExtractedType type)
		{
			return TextTemplateHelper.GetPartialTypeTextTemplate(type, _formatter);
		}

		private void CompileImports()
		{
			foreach (var type in GetReferenceTypes())
			{
				foreach (var property in type.Properties)
				{
					if (property.Type is ExtractedTypeDescriptor extractedType && extractedType.TargetPath != _context.OutputPath)
					{
						if (!_imports.ContainsKey(extractedType.TargetPath))
						{
							_imports.Add(extractedType.TargetPath, new ImportStatement(_context.OutputPath, extractedType.TargetPath, true));
						}
						_imports[extractedType.TargetPath].AddItem(extractedType.Name);
					}
				}
			}
		}
	}
}
