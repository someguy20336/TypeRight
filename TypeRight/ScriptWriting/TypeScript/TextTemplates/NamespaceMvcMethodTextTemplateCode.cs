using TypeRight.CodeModel;
using TypeRight.TypeLocation;
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

		/// <summary>
		/// Gets the controller template text
		/// </summary>
		/// <param name="controllerInfo">The controller info</param>
		/// <param name="context">The script write context</param>
		/// <returns>the script text</returns>
		public string GetText(MvcControllerInfo controllerInfo, ControllerContext context)
		{
			_innerTemplate = new MvcMethodTextTemplateBase();
			_innerTemplate.Initialize(controllerInfo, context, new TypeScriptTypeFormatter(context.ExtractedTypes, new NamespacedTypePrefixResolver()));
			_innerTemplate.PushIndent("\t");
			WebMethodNamespace = context.WebMethodNamespace;
			return TransformText();
		}


	}
}
