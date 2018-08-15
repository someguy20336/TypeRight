using TypeRight.CodeModel;
using TypeRight.Packages;
using TypeRight.TypeProcessing;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

namespace TypeRight.ScriptWriting.TypeScript.TextTemplates
{
	/// <summary>
	/// Generates method for each action on a controller
	/// </summary>
	partial class NamespaceMvcMethodTextTemplate : IControllerTextTemplate
	{

		public string WebMethodNamespace { get; private set; }

		private MvcMethodTextTemplateBase _innerTemplate;

		public string GetText(MvcControllerInfo controllerInfo, ScriptWriteContext context, Uri outputPath)
		{
			_innerTemplate = new MvcMethodTextTemplateBase();
			_innerTemplate.Initialize(controllerInfo, context, new TypeScriptTypeFormatter(context.ExtractedTypes));
			_innerTemplate.PushIndent("\t");
			WebMethodNamespace = context.WebMethodNamespace;
			return TransformText();
		}


	}
}
