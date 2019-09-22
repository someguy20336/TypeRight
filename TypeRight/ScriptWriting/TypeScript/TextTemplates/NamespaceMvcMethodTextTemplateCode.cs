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
			var proc = new ControllerProcessor(controllerInfo, context);
			_innerTemplate = new MvcMethodTextTemplateBase();
			TypeFormatter formatter = new TypeScriptTypeFormatter(context.TypeCollection, new NamespacedTypePrefixResolver(context.EnumNamespace, context.TypeNamespace));
			_innerTemplate.Initialize(proc.CreateModel(formatter), context);
			_innerTemplate.PushIndent("\t");
			WebMethodNamespace = context.WebMethodNamespace;
			return TransformText();
		}


	}
}
