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
		private const string s_importName = "ServerObjects";

		private string _importPath;
        private string _ajaxImportPath;
        private ControllerContext _context;

		private MvcMethodTextTemplateBase _innerTemplate;
		public string GetText(MvcControllerInfo controllerInfo, ControllerContext context, Uri outputPath)
		{
			_innerTemplate = new MvcMethodTextTemplateBase();
			_innerTemplate.Initialize(controllerInfo, context, new PrefixedTypeFormatter(context.ExtractedTypes, s_importName, s_importName));
            _context = context;

			// Get relative import paths
			Uri serverObjectsRelativePath = outputPath.MakeRelativeUri(context.ServerObjectsResultFilepath);

            if (context.HasOwnAjaxFunction)
            {
                Uri ajaxPath = outputPath.MakeRelativeUri(context.AjaxFunctionModulePath);
                _ajaxImportPath = ajaxPath.ToString();
                _ajaxImportPath = _ajaxImportPath.Substring(0, _ajaxImportPath.Length - 3);
            }

			_importPath = serverObjectsRelativePath.ToString();
			_importPath = _importPath.Substring(0, _importPath.Length - 3);  // remove .ts
			return TransformText();
		}
	}
}
