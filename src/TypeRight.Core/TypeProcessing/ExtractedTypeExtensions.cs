using System.Collections.Generic;
using System.Linq;

namespace TypeRight.TypeProcessing
{
	/// <summary>
	/// Extensions for use with enumerable lists of extracted types
	/// </summary>
	public static class ExtractedTypeExtensions
	{
		/// <summary>
		/// Gets the reference types from the list
		/// </summary>
		/// <param name="collection">the extracted type collection</param>
		/// <returns></returns>
		public static IEnumerable<ExtractedReferenceType> GetReferenceTypes(this IEnumerable<ExtractedType> collection)
			=> collection.Where(type => type is ExtractedReferenceType).Cast<ExtractedReferenceType>();

		/// <summary>
		/// Gets the enum types from the list
		/// </summary>
		/// <param name="collection">The collection</param>
		/// <returns></returns>
		public static IEnumerable<ExtractedEnumType> GetEnumTypes(this IEnumerable<ExtractedType> collection)
			=> collection.Where(type => type.NamedType.Flags.IsEnum).Cast<ExtractedEnumType>();

		/// <summary>
		/// Gets the first type from the collection by the name of the type
		/// </summary>
		/// <param name="collection">The collection</param>
		/// <param name="name">The name to get</param>
		/// <returns></returns>
		public static ExtractedReferenceType GetTypeByName(this IEnumerable<ExtractedType> collection, string name)
			=> collection.FirstOrDefault(type => type.Name == name) as ExtractedReferenceType;

		public static bool IsComplexType(this TypeDescriptor typeDescriptor) => typeDescriptor is ExtractedTypeDescriptor type && !type.NamedType.Flags.IsEnum;
	}
}
