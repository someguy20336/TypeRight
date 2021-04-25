using TypeRight.Configuration;
using TypeRight.ScriptWriting.TypeScript;
using TypeRight.ScriptWriting.TypeScript.TextTemplates;

namespace TypeRight.ScriptWriting
{
	public class ScriptTemplateFactory
	{
		private ScriptExtensionsFactory _scriptExtensions;
		public ScriptTemplateFactory(ConfigOptions options)
		{
			_scriptExtensions = new ScriptExtensionsFactory(options.QueryParams);
		}

		public ITypeTextTemplate CreateTypeTextTemplate() => new ModuleTypeTextTemplate();

		public IControllerTextTemplate CreateControllerTextTemplate(ControllerContext context) 
			=> new MvcControllerTextTemplate(context, _scriptExtensions);
	}
}
