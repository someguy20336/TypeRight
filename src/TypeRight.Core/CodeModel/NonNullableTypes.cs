using System.Collections.Generic;

namespace TypeRight.CodeModel;
public class NonNullTypeParameter : ITypeParameter
{
	private readonly ITypeParameter _typeParam;
	public bool IsNullable => false;

	public string Name => _typeParam.Name;

	public NonNullTypeParameter(ITypeParameter typeParam)
	{
		_typeParam = typeParam;
	}

	public IType AsNonNullable() => this;
}

public class NonNullArrayType : IArrayType
{
	private readonly IArrayType _array;
	public IType ElementType => _array.ElementType;

	public bool IsNullable => false;

	public string Name => _array.Name;

	public NonNullArrayType(IArrayType array)
	{
		_array = array;
	}

	public IType AsNonNullable() => this;
}

public class NonNullNamedType : INamedType
{
	private readonly INamedType _namedType;
	public INamedType ConstructedFromType => _namedType.ConstructedFromType;

	public INamedType BaseType => _namedType.BaseType;

	public IReadOnlyList<INamedType> Interfaces => _namedType.Interfaces;

	public IReadOnlyList<IType> TypeArguments => _namedType.TypeArguments;

	public string Comments => _namedType.Comments;

	public IReadOnlyList<IProperty> Properties => _namedType.Properties;

	public IReadOnlyList<IField> Fields => _namedType.Fields;

	public IReadOnlyList<IMethod> Methods => _namedType.Methods;

	public IReadOnlyList<IAttributeData> Attributes => _namedType.Attributes;

	public TypeFlags Flags => _namedType.Flags;

	public string FilePath => _namedType.FilePath;

	public string Name => _namedType.Name;

	public string FullName => _namedType.FullName;

	public bool IsNullable => false;

	public NonNullNamedType(INamedType namedType)
	{
		_namedType = namedType;
	}

	public IType AsNonNullable() => this;
}