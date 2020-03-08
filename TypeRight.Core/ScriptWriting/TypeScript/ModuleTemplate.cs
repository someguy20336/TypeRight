using TypeRight.ScriptWriting.TypeScript.TextTemplates;

namespace TypeRight.ScriptWriting.TypeScript
{
	/// <summary>
	/// A template that uses the module format
	/// </summary>
	public class ModuleTemplate : IScriptTemplate
	{
		/// <summary>
		/// Creates the controller text template
		/// </summary>
		/// <returns></returns>
		public IControllerTextTemplate CreateControllerTextTemplate() => new ModuleMvcMethodTextTemplate();

		/// <summary>
		/// Creates the type text template
		/// </summary>
		/// <returns></returns>
		public ITypeTextTemplate CreateTypeTemplate() => new ModuleTypeTextTemplate();

	}
}
