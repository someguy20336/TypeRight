using TypeRight.Packages;

namespace TypeRight.ScriptWriting
{
	/// <summary>
	/// An object that can generate scripts from a <see cref="ScriptPackage"/>
	/// </summary>
	public interface IScriptTemplate
	{
		/// <summary>
		/// Creates the type template
		/// </summary>
		/// <returns></returns>
		ITypeTextTemplate CreateTypeTemplate();

		/// <summary>
		/// Creates a controller template
		/// </summary>
		/// <returns></returns>
		IControllerTextTemplate CreateControllerTextTemplate();
	}
}
