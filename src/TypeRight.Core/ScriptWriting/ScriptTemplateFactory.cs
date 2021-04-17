using System;
using System.Collections.Generic;
using System.Text;
using TypeRight.ScriptWriting.TypeScript;

namespace TypeRight.ScriptWriting
{
	public class ScriptTemplateFactory
	{
		public static IScriptTemplate GetTemplate()
		{
			return new ModuleTemplate();
		}
	}
}
