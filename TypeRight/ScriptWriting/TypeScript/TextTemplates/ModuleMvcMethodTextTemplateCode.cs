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

		private MvcMethodTextTemplateBase _innerTemplate;
		public string GetText(MvcControllerInfo controllerInfo, ScriptWriteContext context, Uri outputPath)
		{
			_innerTemplate = new MvcMethodTextTemplateBase();
			_innerTemplate.Initialize(controllerInfo, context, new PrefixedTypeFormatter(context.ExtractedTypes, s_importName, s_importName));

			// Get relative import path
			Uri serverObjects = new Uri(context.ServerObjectsResultFilepath);
			Uri relativePath = outputPath.MakeRelativeUri(serverObjects);

			_importPath = relativePath.ToString();
			_importPath = _importPath.Substring(0, _importPath.Length - 3);  // remove .ts
			return TransformText();
		}
	}
}
