using System.Collections.Generic;
using System.Linq;
using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting.TypeScript.TextTemplates
{
	internal partial class ModuleTypeTextTemplate : ITypeTextTemplate
	{
		private ImportManager _imports;
		private TypeFormatter _formatter;

		private TypeWriteContext _context;
		private ImportModuleNameStyle _importModuleNameStyle;

		public ModuleTypeTextTemplate(ImportModuleNameStyle nameStyle)
        {
			_importModuleNameStyle = nameStyle;
        }

		/// <summary>
		/// Gets the script for the extracted types
		/// </summary>
		/// <param name="context">The context for writing the script</param>
		/// <returns>The script text</returns>
		public string GetText(TypeWriteContext context)
		{
			_context = context;

			_imports = ImportManager.FromTypes(_context.IncludedTypes, _context.OutputPath, _importModuleNameStyle);
			_formatter = new TypeScriptTypeFormatter(context.TypeCollection, new ModuleTypePrefixResolver(_imports));  // Add no prefix to the types

			return TransformText();
		}

		private IEnumerable<ImportStatement> GetImports() => _imports.GetImports().OrderBy(i => i.FromRelativePath);

		private IEnumerable<ExtractedReferenceType> GetReferenceTypes() => _context.IncludedTypes.GetReferenceTypes().OrderBy(type => type.Name);

		private IEnumerable<ExtractedEnumType> GetEnumTypes() => _context.IncludedTypes.GetEnumTypes().OrderBy(type => type.Name);

		private IPartialTypeTextTemplate GetPartialTextTemplate(ExtractedType type)
		{
			return TextTemplateHelper.GetPartialTypeTextTemplate(type, _formatter);
		}
			   	
	}
}
