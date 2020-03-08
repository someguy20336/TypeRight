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
		// This could be better, but eh


		public const string NamespaceKey = "NAMESPACE";

		public const string ModuleKey = "MODULE";


		public static IScriptTemplate GetTemplate(string key)
		{
			string normalized = key?.ToUpper();
			if (string.IsNullOrEmpty(normalized) || normalized == NamespaceKey)
			{
				return new NamespaceTemplate();
			}
			else if (normalized == ModuleKey)
			{
				return new ModuleTemplate();
			}
			else
			{
				throw new InvalidOperationException($"A template type of {key} was not found.");
			}
		}
	}
}
