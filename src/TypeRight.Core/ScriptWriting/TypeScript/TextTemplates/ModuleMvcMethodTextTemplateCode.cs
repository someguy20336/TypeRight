using System.Collections.Generic;
using System.Linq;

namespace TypeRight.ScriptWriting.TypeScript.TextTemplates
{
	partial class ModuleMvcMethodTextTemplate : IControllerTextTemplate
	{
		private MvcMethodTextTemplateBase _innerTemplate;

		private ControllerModel _model;

		private ImportManager _imports;

		/// <summary>
		/// Gets the controller template text
		/// </summary>
		/// <param name="controllerInfo">The controller info</param>
		/// <param name="context">The script write context</param>
		/// <returns>the script text</returns>
		public string GetText(ControllerContext context)
		{
			// This is kinda weird..
			_imports = ImportManager.FromController(context);
			ControllerProcessor controllerProcessor = new ControllerProcessor(context);
			TypeFormatter formatter = new TypeScriptTypeFormatter(context.TypeCollection, new ModuleTypePrefixResolver(_imports));
			_model = controllerProcessor.CreateModel(formatter);

			_innerTemplate = new MvcMethodTextTemplateBase();
			_innerTemplate.Initialize(_model, context);

			return TransformText();
		}


		private IEnumerable<ImportStatement> GetImports() => _imports.GetImports().OrderBy(imp => imp.FromRelativePath);

	}
}
