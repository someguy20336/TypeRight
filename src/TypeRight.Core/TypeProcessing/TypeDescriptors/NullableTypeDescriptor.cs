using System;
using System.Collections.Generic;
using TypeRight.CodeModel;
using TypeRight.ScriptWriting;

namespace TypeRight.TypeProcessing
{
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
		private readonly IType _type;

		/// <summary>
		/// Gets the type argument descriptor
		/// </summary>
		public TypeDescriptor TypeArgument => GetOrCreateTypeArg();

		/// <summary>
		/// Creates a nullable type descriptor
		/// </summary>
		/// <param name="type"></param>
		/// <param name="typeFactory"></param>
		internal NullableTypeDescriptor(IType type, TypeFactory typeFactory) : base(type)
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
				IType useType = _type;
				if (_type is INamedType namedType)
				{
					useType = namedType.ConstructedFromType.FullName == typeof(Nullable<>).FullName
					   ? namedType.TypeArguments[0]
					   : new NonNullNamedType(namedType);
				}
				else if (_type is IArrayType arrayType)
				{
					useType = new NonNullArrayType(arrayType);
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

		private class NonNullArrayType : IArrayType
		{
			private readonly IArrayType _array;

			public NonNullArrayType(IArrayType array)
			{
				_array = array;
			}

			public IType ElementType => _array.ElementType;

			public bool IsNullable => false;

			public string Name => _array.Name;
		}

		private class NonNullNamedType : INamedType
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

			public TypeFlags Flags { get; }

			public string FilePath => _namedType.FilePath;

			public string Name => _namedType.Name;

			public string FullName => _namedType.FullName;

			public NonNullNamedType(INamedType namedType)
			{
				_namedType = namedType;

				Flags = namedType.Flags.WithNullable(false);
			}

		}
	}
}
