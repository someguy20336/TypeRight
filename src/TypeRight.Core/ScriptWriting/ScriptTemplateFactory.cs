using TypeRight.Configuration;
using TypeRight.ScriptWriting.TypeScript;
using TypeRight.ScriptWriting.TypeScript.TextTemplates;

namespace TypeRight.ScriptWriting
{
	public class ScriptTemplateFactory
	{
		private readonly ScriptExtensionsFactory _scriptExtensions;
        private readonly ConfigOptions _options;

        public ScriptTemplateFactory(ConfigOptions options)
		{
			_scriptExtensions = new ScriptExtensionsFactory(options.QueryParams);
            _options = options;
        }

		public ITypeTextTemplate CreateTypeTextTemplate() => new ModuleTypeTextTemplate(_options.ImportModuleNameStyle);

		public IControllerTextTemplate CreateControllerTextTemplate(ControllerContext context) 
			=> new MvcControllerTextTemplate(context, _scriptExtensions, _options.ImportModuleNameStyle);
	}
}
