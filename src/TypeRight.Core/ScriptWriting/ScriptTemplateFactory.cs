using TypeRight.ScriptWriting.TypeScript.TextTemplates;

namespace TypeRight.ScriptWriting
{
	public class ScriptTemplateFactory
	{
		public static ITypeTextTemplate CreateTypeTextTemplate() => new ModuleTypeTextTemplate();

		public static IControllerTextTemplate CreateControllerTextTemplate() => new MvcControllerTextTemplate();
	}
}
