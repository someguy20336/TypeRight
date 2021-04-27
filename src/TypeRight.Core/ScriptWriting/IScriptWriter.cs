namespace TypeRight.ScriptWriting
{
	/// <summary>
	/// An object that writes to a script
	/// </summary>
	internal interface IScriptWriter
	{
		void Write(string text);
		void WriteLine(string text);
		void PushIndent();
		string PopIndent();
	}
}
