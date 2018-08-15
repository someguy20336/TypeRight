namespace TypeRight.Build
{
	/// <summary>
	/// Represents a resolved reference
	/// </summary>
	public class ResolvedReference
	{
		/// <summary>
		/// Gets the full path to the reference
		/// </summary>
		public string ReferencePath { get; private set; }

		/// <summary>
		/// Gets whether this reference embeds interop types
		/// </summary>
		public bool EmbedInteropTypes { get; private set; }

		/// <summary>
		/// Gets the command line args this reference would generate
		/// </summary>
		public string CommandLineArg
		{
			get
			{
				string refType = EmbedInteropTypes ? "link" : "reference";
				return $"/{refType}:\"{ReferencePath}\" ";
			}			
		}

		/// <summary>
		/// Creates a new resolved reference
		/// </summary>
		/// <param name="path">The full path to the reference</param>
		/// <param name="embedInterop">Whether this reference embeds interop types</param>
		public ResolvedReference(string path, bool embedInterop)
		{
			ReferencePath = path;
			EmbedInteropTypes = embedInterop;
		}

		/// <summary>
		/// Makes this class look nice
		/// </summary>
		/// <returns>The string version</returns>
		public override string ToString()
		{
			return CommandLineArg;
		}
	}
}
