namespace TypeRight.VsixContract
{
	/// <summary>
	/// Represents a result for a script generation process
	/// </summary>
	public interface IScriptGenerationResult
	{
		/// <summary>
		/// Gets whether the process was a sucess
		/// </summary>
		bool Success { get; }

		/// <summary>
		/// Gets the error message for the process
		/// </summary>
		string ErrorMessage { get; }

	}
}
