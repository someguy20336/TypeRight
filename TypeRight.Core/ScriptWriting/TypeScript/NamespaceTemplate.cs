using TypeRight.ScriptWriting.TypeScript.TextTemplates;

namespace TypeRight.ScriptWriting.TypeScript
{
	/// <summary>
	/// A script writer that writes typescript files.
	/// </summary>
	public class NamespaceTemplate : IScriptTemplate
	{		
		/// <summary>
		/// Gets the type text template for this template
		/// </summary>
		/// <returns></returns>
		public ITypeTextTemplate CreateTypeTemplate() => new NamespaceTypeTextTemplate();

		/// <summary>
		/// Creates the controller text template
		/// </summary>
		/// <returns></returns>
		public IControllerTextTemplate CreateControllerTextTemplate() => new NamespaceMvcMethodTextTemplate();
	}
}
