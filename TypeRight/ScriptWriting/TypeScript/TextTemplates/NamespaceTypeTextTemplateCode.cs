
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

		private ExtractedTypeCollection _typeCollection;

		private TypeFormatter _formatter;
		
		public string GetText(ExtractedTypeCollection typeCollection)
		{
			_typeCollection = typeCollection;
			_formatter = new TypeScriptTypeFormatter(typeCollection);
			return TransformText();
		}


		/// <summary>
		/// Gets the types by their script namespace
		/// </summary>
		/// <returns>An enumerable list of types by their namespace</returns>
		public IEnumerable<IGrouping<string, ExtractedReferenceType>> GetTypesByNamespace()
		{
			return _typeCollection.GetReferenceTypes().GroupBy(type => type.Namespace);
		}

		private string GetClassDeclaration(ExtractedReferenceType refType) => refType.GetClassDeclaration(_formatter);

		private string GetPropertyType(ExtractedProperty property) => property.Type.FormatType(_formatter);

		/// <summary>
		/// Gets the client script enums by their script namespace
		/// </summary>
		/// <returns>An enumerable list of enums by their namespace</returns>
		public IEnumerable<IGrouping<string, ExtractedEnumType>> GetEnumsByNamespace()
		{
			return _typeCollection.GetEnums().GroupBy(en => en.Namespace);
		}

		private IPartialTypeTextTemplate GetPartialTextTemplate(ExtractedType type)
		{
			IPartialTypeTextTemplate partial = TextTemplateHelper.GetPartialTypeTextTemplate(type, _formatter);
			partial.PushIndent("\t");
			return partial;
		}

	}

}
