using System;
using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting.TypeScript.PartialTextTemplates
{
	partial class EnumTextTemplate : IPartialTypeTextTemplate
	{
		private ExtractedEnumType _enumType;

		public EnumTextTemplate()
		{
		}

		public string GetText(ExtractedType type)
		{
			if (type is ExtractedEnumType enumType)
			{
				_enumType = enumType;
			}
			else
			{
				throw new InvalidOperationException($"Expecting {nameof(ExtractedEnumType)}");
			}
			string text = TransformText();
			_enumType = null;
			return text;
		}

		private string FormatDocs(string text, string indent)
			=> TypeScriptHelper.FormatDocs(text, indent);
	}
}
