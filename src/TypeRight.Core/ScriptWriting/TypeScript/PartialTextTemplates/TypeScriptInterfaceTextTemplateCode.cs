using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting.TypeScript.PartialTextTemplates
{
	partial class TypeScriptInterfaceTextTemplate : IPartialTypeTextTemplate
	{
		private TypeFormatter _formatter;
		private ExtractedInterfaceType _type;

		internal TypeScriptInterfaceTextTemplate(TypeFormatter formatter)
		{
			_formatter = formatter;
		}

		private string GetClassDeclaration() => _type.GetClassDeclaration(_formatter);

		private string GetPropertyType(ExtractedProperty property) => property.Type.FormatType(_formatter);


		/// <summary>
		/// Gets the base class that the given class extends, provided it is actually extracted
		/// </summary>
		/// <returns>The "extends" part of the class definition</returns>
		private string GetExtendsType()
		{
			if (_type.Interfaces.Count() == 0)
			{
				return "";
			}
			return " extends " + string.Join(", ", _type.Interfaces.Select(inter => inter.FormatType(_formatter)));
		}

		public string GetText(ExtractedType type)
		{
			if (type is ExtractedInterfaceType inter)
			{
				_type = inter;
			}
			else
			{
				throw new InvalidOperationException($"Expecting {nameof(ExtractedInterfaceType)}");
			}
			string text = TransformText();
			_type = null;
			return text;
		}
	}
}
