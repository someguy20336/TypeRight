using TypeRight.CodeModel;
using TypeRight.Packages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeRight.TypeProcessing
{
	internal class TypeTable
	{
		/// <summary>
		/// Index of types that are string types in Typescript
		/// </summary>
		private static HashSet<string> s_stringTypes = new HashSet<string>()  // TODO: make immutable
        {
			typeof(string).FullName
		};

		/// <summary>
		/// Index of types that are numeric types in Typescript
		/// </summary>
		private static HashSet<string> s_numericTypes = new HashSet<string>()
		{
			typeof(int).FullName,
			typeof(long).FullName,
			typeof(decimal).FullName,
			typeof(float).FullName,
			typeof(double).FullName
		};

		/// <summary>
		/// Index of types that are boolean types in Typescript
		/// </summary>
		private static HashSet<string> s_booleanTypes = new HashSet<string>()
		{
			typeof(bool).FullName
		};

		/// <summary>
		/// Index of types that are boolean types in Typescript
		/// </summary>
		private static HashSet<string> s_datetimeTypes = new HashSet<string>()
		{
			typeof(DateTime).FullName
		};

		private Dictionary<string, ExtractedType> _extractedTypes = new Dictionary<string, ExtractedType>();
		private TypeDescriptor _stringType;
		private TypeDescriptor _numericType;
		private TypeDescriptor _booleanType;
		private TypeDescriptor _dateTimeType;

		public TypeTable(ScriptPackage package, ProcessorSettings settings)
		{
			Compile(package, settings);
		}

		public bool ContainsNamedType(INamedType namedType)
		{
			return _extractedTypes.ContainsKey(namedType.ConstructedFromType.FullName);
		}


		public ExtractedType FindUserTypeByName(string metadataName)
		{
			if (_extractedTypes.ContainsKey(metadataName))
			{
				return _extractedTypes[metadataName];
			}
			return null;
		}

		public TypeDescriptor LookupType(IType type)
		{
			if (type is INamedType namedType)
			{
				// "User defined" types
				string metadataName = namedType.ConstructedFromType.FullName;
				if (_extractedTypes.ContainsKey(metadataName))
				{
					ExtractedType extractedType = _extractedTypes[metadataName];
					if (extractedType is ExtractedEnumType enumType)
					{
						return new ExtractedEnumTypeDescriptor(namedType, extractedType.Namespace, enumType.UseExtendedSyntax);
					}
					else
					{
						return new NamedReferenceTypeDescriptor(namedType, extractedType.Namespace, this);
					}
				}

				// Anonymous type
				else if (namedType.Flags.IsAnonymousType)
				{
					return new AnonymousTypeDescriptor(namedType, this);
				}

				// Non extracted type
				else if (namedType.Flags.IsEnum)
				{
					return new NonExtractedEnumTypeDescriptor(namedType);
				}

				// Nullable types
				else if (namedType.Flags.IsNullable)
				{
					return new NullableTypeDescriptor(namedType, this);
				}

				// List types
				else if (namedType.Flags.IsList)
				{
					return new ListTypeDescriptor(namedType, this);
				}

				// Dictionary type
				else if (namedType.Flags.IsDictionary)
				{
					return new DictionaryTypeDescriptor(namedType, this);
				}

				// System types- TODO: cache these - flyweight
				else if (s_stringTypes.Contains(metadataName))
				{
					if (_stringType == null)
					{
						_stringType = new StringTypeDescriptor(namedType);
					}
					return _stringType;
				}
				else if (s_numericTypes.Contains(metadataName))
				{
					if (_numericType == null)
					{
						_numericType = new NumericTypeDescriptor(namedType);
					}
					return _numericType;
				}
				else if (s_booleanTypes.Contains(metadataName))
				{
					if (_booleanType == null)
					{
						_booleanType = new BooleanTypeDescriptor(namedType);
					}
					return _booleanType;
				}
				else if (s_datetimeTypes.Contains(metadataName))
				{
					if (_dateTimeType == null)
					{
						_dateTimeType = new DateTimeTypeDescriptor(namedType);
					}
					return _dateTimeType;
				}
			}
			else if (type is ITypeParameter)
			{
				return new TypeParameterDescriptor(type);
			}
			else if (type is IArrayType array)
			{
				return new ArrayTypeDescriptor(array, this);
			}

			return new UnknownTypeDescriptor();
		}


		public ExtractedType LookupExtractedType(IType type)
		{
			if (type is INamedType namedType)
			{
				string metadataName = namedType.ConstructedFromType.FullName;
				if (_extractedTypes.ContainsKey(metadataName))
				{
					return _extractedTypes[metadataName];
				}
			}

			return null;
		}

		private void Compile(ScriptPackage package, ProcessorSettings settings)
		{
			
			foreach (INamedType type in package.NamedTypes)
			{
				string metadataName = type.ConstructedFromType.FullName;
				if (type.Flags.IsInterface)  // Interface
				{
					_extractedTypes.Add(metadataName, new ExtractedInterfaceType(type, settings.TypeNamespace, this));
				}
				else if (type.Flags.IsEnum)
				{
					_extractedTypes.Add(
						metadataName,
						new ExtractedEnumType(type, settings.EnumNamespace, settings.DisplayNameFilter)
						);
				}
				else // class
				{
					_extractedTypes.Add(metadataName, new ExtractedClassType(type, settings.TypeNamespace, this));
				}
			}
		}

		public IEnumerable<ExtractedReferenceType> GetReferenceTypes() => _extractedTypes.Values.Where(type => type is ExtractedReferenceType).Cast<ExtractedReferenceType>();

		public IEnumerable<ExtractedEnumType> GetEnums() => _extractedTypes.Values.Where(type => type.NamedType.Flags.IsEnum).Cast<ExtractedEnumType>();
	}
}
