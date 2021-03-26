using TypeRight.CodeModel;

namespace TypeRight.TypeProcessing
{
	/// <summary>
	/// Represents a class type that is extracted
	/// </summary>
	public class ExtractedClassType : ExtractedReferenceType
	{
		
		internal ExtractedClassType(INamedType namedType, TypeFactory factory, string targetPath) 
			: base(namedType, factory, targetPath)
		{
		}

		/// <summary>
		/// Gets the strategy to retrieve class properties
		/// </summary>
		/// <returns></returns>
		protected override PropertyRetrieveStrategy GetPropertyRetrieveStrategy()
		{
			return new ClassPropertyRetrieveStrategy(TypeFactory);
		}
		
	}
}
