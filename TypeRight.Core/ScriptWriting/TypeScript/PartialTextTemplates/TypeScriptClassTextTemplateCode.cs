using TypeRight.TypeProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeRight.ScriptWriting.TypeScript.PartialTextTemplates
{
	partial class TypeScriptClassTextTemplate : IPartialTypeTextTemplate
	{
		private TypeFormatter _formatter;
		private ExtractedClassType _type;
		
		internal TypeScriptClassTextTemplate(TypeFormatter formatter)
		{
			_formatter = formatter;
		}

		private string GetClassDeclaration() => _type.GetClassDeclaration(_formatter);

		private string GetPropertyType(ExtractedProperty property) => property.Type.FormatType(_formatter);


		/// <summary>
		/// Gets the base class that the given class extends, provided it is actually extracted
		/// </summary>
		/// <returns>The "extends" part of the class definition</returns>
		private string GetExtendsType() => _type.BaseType == null ? "" : $" extends {_type.BaseType.FormatType(_formatter)}";

		public string GetText(ExtractedType type)
		{
			if (type is ExtractedClassType cl)
			{
				_type = cl;
			}
			else
			{
				throw new InvalidOperationException($"Expecting {nameof(ExtractedClassType)}");
			}
			string text = TransformText();
			_type = null;
			return text;
		}
	}
}
