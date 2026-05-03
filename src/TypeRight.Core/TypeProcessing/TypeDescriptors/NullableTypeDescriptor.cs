using System;
using TypeRight.CodeModel;
using TypeRight.ScriptWriting;

namespace TypeRight.TypeProcessing;

/// <summary>
/// A type descriptor for a nullable type
/// </summary>
public class NullableTypeDescriptor : TypeDescriptor
{
	/// <summary>
	/// The type arg
	/// </summary>
	private TypeDescriptor _typeArg;

	/// <summary>
	/// The type table
	/// </summary>
	private readonly TypeFactory _typeFactory;

	/// <summary>
	/// The named type
	/// </summary>
	private readonly IMaybeNullable _type;

	/// <summary>
	/// Gets the type argument descriptor
	/// </summary>
	public TypeDescriptor TypeArgument => GetOrCreateTypeArg();

	/// <summary>
	/// Creates a nullable type descriptor
	/// </summary>
	/// <param name="type"></param>
	/// <param name="typeFactory"></param>
	internal NullableTypeDescriptor(IMaybeNullable type, TypeFactory typeFactory) : base(type)
	{
		_type = type;
		_typeFactory = typeFactory;
	}

	/// <summary>
	/// Gets or creates the type argument
	/// </summary>
	/// <returns></returns>
	private TypeDescriptor GetOrCreateTypeArg()
	{
		if (_typeArg == null)
		{
			IType useType = _type.AsNonNullable();
			if (useType is INamedType namedType)
			{
				useType = namedType.ConstructedFromType.FullName == typeof(Nullable<>).FullName
				   ? namedType.TypeArguments[0]
				   : namedType;
			}

			_typeArg = _typeFactory.LookupType(useType);
		}
		return _typeArg;
	}

	/// <summary>
	/// Formats the type
	/// </summary>
	/// <param name="formatter">The type formatter</param>
	/// <returns>The formatter type</returns>
	public override string FormatType(TypeFormatter formatter)
	{
		return formatter.FormatNullableType(this);
	}
}
