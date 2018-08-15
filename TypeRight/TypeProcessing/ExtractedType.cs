using TypeRight.CodeModel;

namespace TypeRight.TypeProcessing
{
	/// <summary>
	/// Represents an extracted type
	/// </summary>
	public abstract class ExtractedType
	{
		/// <summary>
		/// Gets the named type for this extracted type
		/// </summary>
		public INamedType NamedType { get; }

		/// <summary>
		/// Gets the script namespace for this extracted type
		/// </summary>
		public string Namespace { get; private set; }

		/// <summary>
		/// Gets the comments for the extracted type
		/// </summary>
		public string Comments => NamedType.Comments;

		/// <summary>
		/// Gets the name of the extracted type
		/// </summary>
		public string Name => NamedType.Name;

		/// <summary>
		/// Creates a new extracted type
		/// </summary>
		/// <param name="namedType">The named type</param>
		/// <param name="typeNamespace">The namespace for the script</param>
		protected ExtractedType(INamedType namedType, string typeNamespace)
		{
			NamedType = namedType;
			Namespace = typeNamespace;
		}

		/// <summary>
		/// Pretty print
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return NamedType.ToString();
		}
	}
}
