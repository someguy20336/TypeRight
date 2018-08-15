namespace TypeRight.ScriptGeneration
{
	/// <summary>
	/// Represents a result for a script generation process
	/// </summary>
	public class ScriptGenerationResult : IScriptGenerationResult
	{
		/// <summary>
		/// Gets whether the process was a sucess
		/// </summary>
		public bool Sucess { get; private set; }

		/// <summary>
		/// Gets the error message for the process
		/// </summary>
		public string ErrorMessage { get; private set; }

		/// <summary>
		/// Creates a new script generation result
		/// </summary>
		/// <param name="sucess">True if sucessful</param>
		/// <param name="msg">The error message if not sucessful</param>
		public ScriptGenerationResult(bool sucess, string msg)
		{
			Sucess = sucess;
			ErrorMessage = msg;
		}
	}
}
