using TypeRight.CodeModel;
using System.Collections.Generic;

namespace TypeRight.Packages
{
	/// <summary>
	/// An object that visits types to perform filtering and manipulation for a package
	/// </summary>
	public class TypeVisitor
	{
		/// <summary>
		/// Gets or sets the filter settings for the visitor
		/// </summary>
		public ParseFilterSettings FilterSettings { get; set; } = new ParseFilterSettings();

		/// <summary>
		/// Gets the list of class objects that were found
		/// </summary>
		public List<INamedType> FoundTypes { get; } = new List<INamedType>();
		
		/// <summary>
		/// Gets the list of controllers that were found
		/// </summary>
		public List<INamedType> FoundControllers { get; } = new List<INamedType>();
				
		/// <summary>
		/// Performs a visit for the given named type
		/// </summary>
		/// <param name="namedType">The named type</param>
		public void Visit(INamedType namedType)
		{
			if (FilterSettings.ClassFilter.Evaluate(namedType)
				|| FilterSettings.EnumFilter.Evaluate(namedType))
			{
				FoundTypes.Add(namedType);
			}
			else if (FilterSettings.ControllerFilter.Evaluate(namedType))
			{
				FoundControllers.Add(namedType);
			}
		}

		/// <summary>
		/// Visits an external type, which is always assumed to be a found class type
		/// </summary>
		/// <param name="namedType">The named type object</param>
		public void VisitExternalType(INamedType namedType)
		{
			FoundTypes.Add(namedType);
		}
	}
}
