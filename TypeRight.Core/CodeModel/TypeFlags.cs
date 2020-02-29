namespace TypeRight.CodeModel
{
	/// <summary>
	/// Represents different attributes of a given type
	/// </summary>
	public class TypeFlags
	{
		/// <summary>
		/// Gets whether this type is an enum
		/// </summary>
		public bool IsEnum { get; }

		/// <summary>
		/// Gets whether this type is nullable
		/// </summary>
		public bool IsNullable { get; }

		/// <summary>
		/// Gets whether this type is an array
		/// </summary>
		public bool IsArray { get; }

		/// <summary>
		/// Gets whether this type is a list
		/// </summary>
		public bool IsList { get; }

		/// <summary>
		/// Gets whether this type is a dictionary
		/// </summary>
		public bool IsDictionary { get; }

		/// <summary>
		/// Gets whether this type is an anonymous type
		/// </summary>
		public bool IsAnonymousType { get; }

		/// <summary>
		/// Gets whether this type is an interface
		/// </summary>
		public bool IsInterface { get; }

		/// <summary>
		/// Creates a new type flags object
		/// </summary>
		/// <param name="isEnum"></param>
		/// <param name="isNullable"></param>
		/// <param name="isArray"></param>
		/// <param name="isList"></param>
		/// <param name="isDictionary"></param>
		/// <param name="isAnonymous"></param>
		/// <param name="isInterface">Flag for whethe this type is an interface</param>
		public TypeFlags(
			bool isEnum = false,
			bool isNullable = false,
			bool isArray = false,
			bool isList = false,
			bool isDictionary = false,
			bool isAnonymous = false,
			bool isInterface = false
			)
		{
			IsEnum = isEnum;
			IsNullable = isNullable;
			IsArray = isArray;
			IsList = isList;
			IsDictionary = isDictionary;
			IsAnonymousType = isAnonymous;
			IsInterface = isInterface;
		}
	}
}
