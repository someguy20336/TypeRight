using TypeRight.CodeModel;

namespace TypeRight.TypeProcessing
{
	/// <summary>
	/// Represents a class type that is extracted
	/// </summary>
	public class ExtractedClassType : ExtractedReferenceType
	{
		
		internal ExtractedClassType(INamedType namedType, string typeNamespace, TypeTable typeTable, string targetPath) 
			: base(namedType, typeNamespace, typeTable, targetPath)
		{
		}

		/// <summary>
		/// Gets the strategy to retrieve class properties
		/// </summary>
		/// <returns></returns>
		protected override PropertyRetrieveStrategy GetPropertyRetrieveStrategy()
		{
			return new ClassPropertyRetrieveStrategy(TypeTable);
		}
		
	}
}
