using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeRight.Configuration;
using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting.TypeScript.TextTemplates
{
	partial class ModuleMvcMethodTextTemplate : IControllerTextTemplate
	{
		private MvcMethodTextTemplateBase _innerTemplate;

		private ControllerModel _model;

		/// <summary>
		/// Gets the controller template text
		/// </summary>
		/// <param name="controllerInfo">The controller info</param>
		/// <param name="context">The script write context</param>
		/// <returns>the script text</returns>
		public string GetText(ControllerContext context)
		{
			// This is kinda weird..
			ControllerProcessor controllerProcessor = new ControllerProcessor(context);
			TypeFormatter formatter = new TypeScriptTypeFormatter(context.TypeCollection, new ModuleTypePrefixResolver(controllerProcessor.Imports));
			_model = controllerProcessor.CreateModel(formatter);

			_innerTemplate = new MvcMethodTextTemplateBase();
			_innerTemplate.Initialize(_model, context);

			return TransformText();
		}


		private IEnumerable<ImportStatement> GetImports() => _model.Imports.OrderBy(imp => imp.FromRelativePath);

	}
}
