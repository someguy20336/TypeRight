using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
	}
}
