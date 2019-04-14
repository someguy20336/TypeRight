﻿using TypeRight.CodeModel;

namespace TypeRight.TypeProcessing
{
	/// <summary>
	/// Represents an extracted interface
	/// </summary>
	public class ExtractedInterfaceType : ExtractedReferenceType
	{

		internal ExtractedInterfaceType(INamedType namedType, TypeTable typeTable, string targetPath) 
			: base(namedType, typeTable, targetPath)
		{
		}

		/// <summary>
		/// Gets the interface property retrieve strategy
		/// </summary>
		/// <returns></returns>
		protected override PropertyRetrieveStrategy GetPropertyRetrieveStrategy()
		{
			return new InterfacePropertyRetrieveStrategy(TypeTable);
		}
		
	}
}
