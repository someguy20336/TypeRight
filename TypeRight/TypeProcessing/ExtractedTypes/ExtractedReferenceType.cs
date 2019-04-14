using System.Collections.Generic;
using TypeRight.CodeModel;
using TypeRight.ScriptWriting;

namespace TypeRight.TypeProcessing
{
	/// <summary>
	/// An extracted reference type
	/// </summary>
	public abstract class ExtractedReferenceType : ExtractedType
	{

		private IEnumerable<ExtractedProperty> _properties;

		/// <summary>
		/// Gets the properties of this type
		/// </summary>
		public IEnumerable<ExtractedProperty> Properties => GetOrCreateProperties();

		internal TypeTable TypeTable { get; private set; }

		private TypeDescriptor _baseType;

		private IEnumerable<NamedReferenceTypeDescriptor> _interfaces;

		private bool _baseTypeProcessed = false;

		/// <summary>
		/// Gets the first extracted base type.  If none of the base types of the original type hierarchy are extracted,
		/// or the type does have a base type, this property will be null
		/// </summary>
		public TypeDescriptor BaseType => GetOrCreateBaseType();

		/// <summary>
		/// Gets the list of interfaces for this type
		/// </summary>
		public IEnumerable<NamedReferenceTypeDescriptor> Interfaces => GetOrCreateInterfaces();

		internal ExtractedReferenceType(INamedType namedType, string typeNamespace, TypeTable typeTable, string targetPath) : base(namedType, typeNamespace, targetPath)
		{
			TypeTable = typeTable;
		}
		
		/// <summary>
		/// Gets the stragegy used for retrieving properties
		/// </summary>
		/// <returns></returns>
		protected abstract PropertyRetrieveStrategy GetPropertyRetrieveStrategy();

		private IEnumerable<ExtractedProperty> GetOrCreateProperties()
		{
			if (_properties == null)
			{
				PropertyRetrieveStrategy strategy = GetPropertyRetrieveStrategy();
				_properties = strategy.GetProperties(this);
			}
			return _properties;
		}


		private TypeDescriptor GetOrCreateBaseType()
		{
			if (!_baseTypeProcessed)
			{
				INamedType testBaseType = NamedType.BaseType;

				while (testBaseType != null && _baseType == null)
				{
					if (TypeTable.ContainsNamedType(testBaseType))
					{
						_baseType = TypeTable.LookupType(testBaseType);
					}
					testBaseType = testBaseType.BaseType;
				}
				_baseTypeProcessed = true;
			}

			return _baseType;
		}

		private IEnumerable<NamedReferenceTypeDescriptor> GetOrCreateInterfaces()
		{
			if (_interfaces == null)
			{
				List<NamedReferenceTypeDescriptor> interfaces = new List<NamedReferenceTypeDescriptor>();
				foreach (INamedType namedType in NamedType.Interfaces)
				{
					if (TypeTable.ContainsNamedType(namedType))
					{
						interfaces.Add(TypeTable.LookupType(namedType) as NamedReferenceTypeDescriptor);
					}
				}

				_interfaces = interfaces;
			}
			return _interfaces;
		}

		/// <summary>
		/// Gets the class declaration using the given type formatter
		/// </summary>
		/// <param name="formatter">The formatter</param>
		/// <returns>The formatted declaration</returns>
		public string GetClassDeclaration(TypeFormatter formatter)
		{
			return formatter.FormatNamedTypeDeclaration(this);  // Hack?
		}
		
	}
}
