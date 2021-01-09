using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeRight.ScriptWriting.TypeScript.PartialTextTemplates;
using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting.TypeScript.TextTemplates
{
	static class TextTemplateHelper
	{

		public static IPartialTypeTextTemplate GetPartialTypeTextTemplate(ExtractedType type,  TypeFormatter formatter)
		{
			
			IPartialTypeTextTemplate partial;
			if (type is ExtractedClassType cl)
			{
				partial = new TypeScriptClassTextTemplate(formatter);
			}
			else if (type is ExtractedInterfaceType inter)
			{
				partial = new TypeScriptInterfaceTextTemplate(formatter);
			}
			else if (type is ExtractedEnumType enumType)
			{
				partial = new EnumTextTemplate();
			}
			else
			{
				throw new NotImplementedException("Type does not have an implement partial template");
			}
			return partial;
		}
	}
}
