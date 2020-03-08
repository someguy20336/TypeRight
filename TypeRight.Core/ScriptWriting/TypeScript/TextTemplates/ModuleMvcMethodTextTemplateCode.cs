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
		private ControllerContext _context;

		private MvcMethodTextTemplateBase _innerTemplate;

		private ControllerModel _model;

		/// <summary>
		/// Gets the controller template text
		/// </summary>
		/// <param name="controllerInfo">The controller info</param>
		/// <param name="context">The script write context</param>
		/// <returns>the script text</returns>
		public string GetText(MvcControllerInfo controllerInfo, ControllerContext context)
		{
			_context = context;

			// This is kinda weird..
			ControllerProcessor controllerProcessor = new ControllerProcessor(controllerInfo, context);
			TypeFormatter formatter = new TypeScriptTypeFormatter(context.TypeCollection, new ModuleTypePrefixResolver(controllerProcessor.Imports));
			_model = controllerProcessor.CreateModel(formatter);

			_innerTemplate = new MvcMethodTextTemplateBase();
			_innerTemplate.Initialize(_model, context);

			return TransformText();
		}


		private IEnumerable<ImportStatement> GetImports() => _model.Imports.OrderBy(imp => imp.FromRelativePath);

	}
}
