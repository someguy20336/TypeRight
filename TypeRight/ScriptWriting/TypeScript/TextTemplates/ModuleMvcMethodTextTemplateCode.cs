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
				ImportStatement ajaxImport = new ImportStatement(context.OutputPath, context.FetchFunctionModulePath, false);
				ajaxImport.AddItem(context.FetchFunctionName);
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

			// Additional imports
			for (int i = 0; i < _context.AdditionalImports?.Count; i++)
			{
				ImportDefinition def = _context.AdditionalImports[i];
				string importPath = PathUtils.ResolveRelativePath(_context.OutputPath, def.Path);

				ImportStatement statement = new ImportStatement(_context.OutputPath, importPath, def.UseAlias);

				if (def.Items != null)
				{
					foreach (var item in def.Items)
					{
						statement.AddItem(item);
					}
				}				

				_imports.Add("custom" + i, statement);
			}
		}

		private void TryAddImport(TypeDescriptor type)
		{
			TypeScriptHelper.TryAddToImports(_imports, type, _context.OutputPath);
		}
	}
}
