using System.Collections.Generic;
using System.Linq;
using TypeRight.ScriptWriting.TypeScript.PartialTextTemplates;
using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting.TypeScript.TextTemplates
{
	internal partial class MvcControllerTextTemplate : IControllerTextTemplate, IScriptWriter
	{
		private readonly ImportManager _imports;
		private readonly ControllerContext _context;
		private readonly TypeFormatter _formatter;
		private readonly ScriptExtensionsFactory _scriptExtensions;

		private readonly IEnumerable<IScriptExtension> _postScriptExtensions;

		public MvcControllerTextTemplate(ControllerContext context, ScriptExtensionsFactory extensionsFactory, ImportModuleNameStyle nameStyle)
		{
			_context = context;
			_scriptExtensions = extensionsFactory;

			_imports = ImportManager.FromControllerContext(context, nameStyle);
			_formatter = new TypeScriptTypeFormatter(context.TypeCollection, new ModuleTypePrefixResolver(_imports));
			_postScriptExtensions = _scriptExtensions.CreatePostControllerScript(context);
		}

		/// <summary>
		/// Gets the controller template text
		/// </summary>
		/// <param name="context">The script write context</param>
		/// <returns>the script text</returns>
		public string GetText()
		{

			return TransformText();
		}


		private IEnumerable<ImportStatement> GetImports() => _imports.GetImports().OrderBy(imp => imp.FromRelativePath);

		/// <summary>
		/// Gets a list of all actions ordered by name
		/// </summary>
		/// <returns>An enumerable list of actions</returns>
		private IEnumerable<MvcAction> GetActions() => _context.Actions.OrderBy(act => act.Name);

		private MvcActionTextTemplate CreateTemplateForAction(MvcAction action) 
			=> new MvcActionTextTemplate(action, _scriptExtensions, _context.FetchFunctionResolver.Resolve(action.RequestMethod.Name), _formatter);

		public void PushIndent()
		{
			PushIndent("\t");
		}

		private void RunPostScriptExtensions()
		{
			foreach (IScriptExtension ext in _postScriptExtensions)
			{
				ext.Write(this);
			}
		}


	}
}
