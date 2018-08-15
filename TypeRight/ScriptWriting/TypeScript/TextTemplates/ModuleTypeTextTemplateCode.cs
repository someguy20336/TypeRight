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
		private ExtractedTypeCollection _typeCollection;

		private TypeFormatter _formatter;

		public string GetText(ExtractedTypeCollection extractedTypeCollection)
		{
			_typeCollection = extractedTypeCollection;
			_formatter = new PrefixedTypeFormatter(extractedTypeCollection, "", "");  // Add no prefix to the types
			return TransformText();
		}

		private IEnumerable<ExtractedReferenceType> GetReferenceTypes() => _typeCollection.GetReferenceTypes().OrderBy(type => type.Name);

		private IEnumerable<ExtractedEnumType> GetEnumTypes() => _typeCollection.GetEnums().OrderBy(type => type.Name);

		private IPartialTypeTextTemplate GetPartialTextTemplate(ExtractedType type)
		{
			return TextTemplateHelper.GetPartialTypeTextTemplate(type, _formatter);
		}
	}
}
