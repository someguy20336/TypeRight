using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting
{
	/// <summary>
	/// Base class for writing scripts
	/// </summary>
	public abstract class ScriptWriteContext
	{
		/// <summary>
		/// Gets or sets the output path
		/// </summary>
		public string OutputPath { get; set; }

		/// <summary>
		/// Gets or sets the type collection
		/// </summary>
		public ExtractedTypeCollection TypeCollection { get; set; }

		/// <summary>
		/// Gets the namespace used by the reference types for the namespaced method
		/// </summary>
		public string TypeNamespace { get; set; }

		/// <summary>
		/// Gest the namespace used by enums for the namespaced method
		/// </summary>
		public string EnumNamespace { get; set; }
	}
}
