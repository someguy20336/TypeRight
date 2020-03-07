namespace TypeRight
{
	/// <summary>
	/// Represents a result for a script generation process
	/// </summary>
	public class ScriptGenerationResult
	{
		/// <summary>
		/// Gets whether the process was a sucess
		/// </summary>
		public bool Success { get; private set; }

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
			Success = sucess;
			ErrorMessage = msg;
		}
	}
}
