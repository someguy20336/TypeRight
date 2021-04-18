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
