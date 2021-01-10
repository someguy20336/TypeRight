using TypeRight.CodeModel;

namespace TypeRight.TypeProcessing
{
	/// <summary>
	/// A descriptor for a type that is extracted
	/// </summary>
	public abstract class ExtractedTypeDescriptor : TypeDescriptor
	{
		/// <summary>
		/// Gets the named type
		/// </summary>
		public INamedType NamedType { get; }

		/// <summary>
		/// Gets the name of the type
		/// </summary>
		public string Name => NamedType.Name;
		
		/// <summary>
		/// Gets the full target path for this extracted type.
		/// </summary>
		public string TargetPath { get; set; }

		/// <summary>
		/// Creates a new extracted type descriptor
		/// </summary>
		/// <param name="namedType">The named type</param>
		/// <param name="targetPath">The target path of the type</param>
		protected ExtractedTypeDescriptor(INamedType namedType, string targetPath) : base (namedType)
		{
			NamedType = namedType;
			TargetPath = targetPath;
		}
	}
}
