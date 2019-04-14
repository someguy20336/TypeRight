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
		/// Gets the the target path for this type
		/// </summary>
		public string TargetPath { get; }

		/// <summary>
		/// Creates a new extracted type
		/// </summary>
		/// <param name="namedType">The named type</param>
		/// <param name="typeNamespace">The namespace for the script</param>
		/// <param name="targetPath">The target result path for this type</param>
		protected ExtractedType(INamedType namedType, string typeNamespace, string targetPath)
		{
			NamedType = namedType;
			Namespace = typeNamespace;
			TargetPath = targetPath;
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
