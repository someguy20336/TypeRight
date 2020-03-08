using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeRight.ScriptWriting.TypeScript;

namespace TypeRight.ScriptWriting
{
	class ScriptTemplateFactory
	{
		public static IScriptTemplate GetTemplate()
		{
			return new ModuleTemplate();
		}
	}
}
