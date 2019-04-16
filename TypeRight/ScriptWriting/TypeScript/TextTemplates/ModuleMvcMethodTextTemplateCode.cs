using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting.TypeScript.TextTemplates
{
	partial class ModuleMvcMethodTextTemplate : IControllerTextTemplate
	{
		private Dictionary<string, ImportStatement> _imports = new Dictionary<string, ImportStatement>();

        private ControllerContext _context;

		private MvcMethodTextTemplateBase _innerTemplate;

		/// <summary>
		/// Gets the controller template text
		/// </summary>
		/// <param name="controllerInfo">The controller info</param>
		/// <param name="context">The script write context</param>
		/// <returns>the script text</returns>
		public string GetText(MvcControllerInfo controllerInfo, ControllerContext context)
		{
            _context = context;

			// Get relative import paths
            if (context.HasOwnAjaxFunction)
            {
				ImportStatement ajaxImport = new ImportStatement(context.OutputPath, context.AjaxFunctionModulePath, false);
				ajaxImport.AddItem(context.AjaxFunctionName);
				_imports.Add("ajax", ajaxImport);
            }

			CompileImports(controllerInfo);

			_innerTemplate = new MvcMethodTextTemplateBase();
			_innerTemplate.Initialize(controllerInfo, context, new TypeScriptTypeFormatter(context.TypeCollection, new ModuleTypePrefixResolver(_imports)));

			return TransformText();
		}


		private IEnumerable<ImportStatement> GetImports() => _imports.Values;

		private void CompileImports(MvcControllerInfo controllerInfo)
		{
			foreach (var action in controllerInfo.Actions)
			{
				TryAddImport(action.ReturnType);
				foreach (var param in action.Parameters)
				{
					TryAddImport(param.Type);
				}
			}
		}

		private void TryAddImport(TypeDescriptor type)
		{
			TypeScriptHelper.TryAddToImports(_imports, type, _context.OutputPath);
		}
	}
}
