using TypeRight.CodeModel;
using System;
using System.Collections;
using System.Collections.Generic;
using TypeRight.TypeFilters;
using TypeRight.Attributes;

namespace TypeRight.TypeProcessing
{
	internal class TypeTable : IEnumerable<ExtractedType>   // TODO: should this be renamed to more of a "context" thing?
	{
		internal ProcessorSettings Settings { get; }

		private static TypeFilter s_enumDisplayNameFilter = new HasInterfaceOfTypeFilter(typeof(IEnumDisplayNameProvider).FullName);

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
			typeof(short).FullName,
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

		public TypeTable(ProcessorSettings settings)
		{
			Settings = settings;
		}

		public bool ContainsNamedType(INamedType namedType)
		{
			return _extractedTypes.ContainsKey(namedType.ConstructedFromType.FullName);
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
						return new ExtractedEnumTypeDescriptor(namedType, enumType.UseExtendedSyntax, extractedType.TargetPath);
					}
					else
					{
						return new NamedReferenceTypeDescriptor(namedType, this, extractedType.TargetPath);
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


		public void AddNamedType(INamedType type, string targetPath = null)
		{
			if (string.IsNullOrEmpty(targetPath))
			{
				targetPath = Settings.DefaultResultPath;
			}
			else
			{
				targetPath = PathUtils.ResolveRelativePath(Settings.ProjectPath, targetPath);
			}

			string metadataName = type.ConstructedFromType.FullName;
			if (type.Flags.IsInterface)  // Interface
			{
				_extractedTypes.Add(metadataName, new ExtractedInterfaceType(type, this, targetPath));
			}
			else if (type.Flags.IsEnum)
			{
				_extractedTypes.Add(
					metadataName,
					new ExtractedEnumType(type, s_enumDisplayNameFilter, targetPath)
					);
			}
			else // class
			{
				_extractedTypes.Add(metadataName, new ExtractedClassType(type, this, targetPath));
			}
		}
		
		public IEnumerator<ExtractedType> GetEnumerator()
		{
			return _extractedTypes.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
