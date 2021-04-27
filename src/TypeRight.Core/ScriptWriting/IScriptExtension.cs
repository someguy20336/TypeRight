namespace TypeRight.ScriptWriting
{
	/// <summary>
	/// Extends a script by adding text
	/// </summary>
	internal interface IScriptExtension
	{
		void Write(IScriptWriter writer);
	}
}
