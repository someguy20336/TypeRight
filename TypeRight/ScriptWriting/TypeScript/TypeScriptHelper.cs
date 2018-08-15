using System.Collections.Generic;
using System.Linq;

namespace TypeRight.ScriptWriting.TypeScript
{
	/// <summary>
	/// A type translator for typescript files
	/// </summary>
	public class TypeScriptHelper
	{
		/// <summary>
		/// The typescript string type name
		/// </summary>
		public const string StringTypeName = "string";

		/// <summary>
		/// The typescript "any" type name
		/// </summary>
		public const string AnyTypeName = "any";

		/// <summary>
		/// The typescript "number" type name
		/// </summary>
		public const string NumericTypeName = "number";

		/// <summary>
		/// The typescript "boolean" type name
		/// </summary>
		public const string BooleanTypeName = "boolean";

		/// <summary>
		/// Builds the anonymous type entry
		/// </summary>
		/// <param name="namesAndTypes">A dictionary of the names and types for the anonymous type</param>
		/// <returns>The typescript name for an anonymous type</returns>
		public static string BuildAnonymousType(Dictionary<string, string> namesAndTypes)
		{
			IEnumerable<string> orderedProps = namesAndTypes.OrderBy(val => val.Key).Select(kv => kv.Key + ": " + kv.Value);
			return $"{{ { string.Join(", ", orderedProps) } }}";
		}

		/// <summary>
		/// Formats a dictionary type for typescript
		/// </summary>
		/// <param name="keyTypeName">The name of the key type</param>
		/// <param name="valTypeName">The name of the value type</param>
		/// <returns>The formatted dictionary type for typesript</returns>
		public static string FormatDictionaryType(string keyTypeName, string valTypeName)
		{
			// format is { [key: keyType]: valueType }
			return $"{{ [key: {keyTypeName}]: {valTypeName} }}";
		}

	}
}
