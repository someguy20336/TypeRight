using System.Collections.Generic;
using System.Linq;
using TypeRight.ScriptWriting.TypeScript.PartialTextTemplates;
using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting.TypeScript.TextTemplates
{
	internal partial class MvcControllerTextTemplate : IControllerTextTemplate
	{
		private MvcActionTextTemplate _innerTemplate;

		private ImportManager _imports;
		private ControllerContext _context;

		/// <summary>
		/// Gets the controller template text
		/// </summary>
		/// <param name="controllerInfo">The controller info</param>
		/// <param name="context">The script write context</param>
		/// <returns>the script text</returns>
		public string GetText(ControllerContext context)
		{
			_context = context;
			_imports = ImportManager.FromController(context);			

			_innerTemplate = new MvcActionTextTemplate(context, _imports);

			return TransformText();
		}


		private IEnumerable<ImportStatement> GetImports() => _imports.GetImports().OrderBy(imp => imp.FromRelativePath);

		/// <summary>
		/// Gets a list of all actions ordered by name
		/// </summary>
		/// <returns>An enumerable list of actions</returns>
		private IEnumerable<MvcActionInfo> GetActions() => _context.Controller.Actions.OrderBy(act => act.Name);
	}
}
