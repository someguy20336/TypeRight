﻿using System;
using TypeRight.ScriptWriting.TypeScript.PartialTextTemplates;
using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting.TypeScript.TextTemplates
{
	internal static class TextTemplateHelper
	{

		public static IPartialTypeTextTemplate GetPartialTypeTextTemplate(ExtractedType type,  TypeFormatter formatter)
		{
			
			IPartialTypeTextTemplate partial;
			if (type is ExtractedClassType)
			{
				partial = new TypeScriptClassTextTemplate(formatter);
			}
			else if (type is ExtractedInterfaceType)
			{
				partial = new TypeScriptInterfaceTextTemplate(formatter);
			}
			else if (type is ExtractedEnumType)
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
