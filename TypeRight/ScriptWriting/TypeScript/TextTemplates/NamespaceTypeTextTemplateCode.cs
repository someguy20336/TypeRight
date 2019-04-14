
using TypeRight.TypeProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using TypeRight.ScriptWriting.TypeScript.PartialTextTemplates;

namespace TypeRight.ScriptWriting.TypeScript.TextTemplates
{
	/// <summary>
	/// Class used for generating typescript objects from a template
	/// </summary>
	internal partial class NamespaceTypeTextTemplate : ITypeTextTemplate
	{
		private IEnumerable<ExtractedType> _types;
		private ExtractedTypeCollection _typeCollection;
		private TypeWriteContext _context;

		private TypeFormatter _formatter;
		/// <summary>
		/// Gets the script for the extracted types
		/// </summary>
		/// <param name="context">The context for writing the script</param>
		/// <returns>The script text</returns>
		public string GetText(TypeWriteContext context)
		{
			_types = context.IncludedTypes;
			_typeCollection = context.TypeCollection;
			_context = context;
			_formatter = new TypeScriptTypeFormatter(_typeCollection, new NamespacedTypePrefixResolver(context.EnumNamespace, context.TypeNamespace));
			return TransformText();
		}  


		/// <summary>
		/// Gets the types by their script namespace
		/// </summary>
		/// <returns>An enumerable list of types by their namespace</returns>
		public IEnumerable<IGrouping<string, ExtractedReferenceType>> GetTypesByNamespace()
		{
			return _types.GetReferenceTypes().GroupBy(type => _context.TypeNamespace);		// yea, who cares
		}

		private string GetClassDeclaration(ExtractedReferenceType refType) => refType.GetClassDeclaration(_formatter);

		private string GetPropertyType(ExtractedProperty property) => property.Type.FormatType(_formatter);

		/// <summary>
		/// Gets the client script enums by their script namespace
		/// </summary>
		/// <returns>An enumerable list of enums by their namespace</returns>
		public IEnumerable<IGrouping<string, ExtractedEnumType>> GetEnumsByNamespace()
		{
			return _types.GetEnumTypes().GroupBy(en => _context.EnumNamespace);	// also who cares
		}

		private IPartialTypeTextTemplate GetPartialTextTemplate(ExtractedType type)
		{
			IPartialTypeTextTemplate partial = TextTemplateHelper.GetPartialTypeTextTemplate(type, _formatter);
			partial.PushIndent("\t");
			return partial;
		}

	}

}
