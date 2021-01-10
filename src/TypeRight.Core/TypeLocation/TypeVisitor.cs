using TypeRight.CodeModel;
using System.Collections.Generic;
using TypeRight.TypeProcessing;

namespace TypeRight.TypeLocation
{
	/// <summary>
	/// An object that visits types to perform filtering and manipulation for a package
	/// </summary>
	public class TypeVisitor : ITypeVisitor
	{
		/// <summary>
		/// Gets or sets the filter settings for the visitor
		/// </summary>
		public ParseFilterSettings FilterSettings { get; } = new ParseFilterSettings();

		/// <summary>
		/// Gets the resulting type collection 
		/// </summary>
		public ExtractedTypeCollection TypeCollection { get; }

		/// <summary>
		/// Creates a new type visitor with the given settings
		/// </summary>
		/// <param name="settings">The processing settings</param>
		public TypeVisitor(ProcessorSettings settings)
		{
			TypeCollection = new ExtractedTypeCollection(settings);
		}

		/// <summary>
		/// Performs a visit for the given named type
		/// </summary>
		/// <param name="namedType">The named type</param>
		/// <param name="targetPath">The target path for the type</param>
		public void Visit(INamedType namedType, string targetPath = null)
		{
			if (FilterSettings.ClassFilter.Evaluate(namedType)
				|| FilterSettings.EnumFilter.Evaluate(namedType))
			{
				TypeCollection.RegisterType(namedType, targetPath);
			}
			else if (FilterSettings.ControllerFilter.Evaluate(namedType))
			{
				TypeCollection.RegisterController(namedType);
			}
		}

		/// <summary>
		/// Visits an external type, which is always assumed to be a found class type
		/// </summary>
		/// <param name="namedType">The named type object</param>
		/// <param name="targetPath">The target path for the external type.  If not specified, the default will be used</param>
		public void VisitExternalType(INamedType namedType, string targetPath = null)
		{
			TypeCollection.RegisterType(namedType, targetPath);
		}

	}
}
