using TypeRight.CodeModel;
using System.Collections.Generic;
using System.Linq;

namespace TypeRight.TypeProcessing
{
	/// <summary>
	/// Retrieve strategies for getting properties for a named type
	/// </summary>
	public abstract class PropertyRetrieveStrategy
	{

		internal TypeFactory TypeFactory { get; private set; }

		internal PropertyRetrieveStrategy(TypeFactory typeFactory)
		{
			TypeFactory = typeFactory;
		}

		/// <summary>
		/// Gets the properties for a type
		/// </summary>
		/// <param name="namedType"></param>
		/// <returns></returns>
		public abstract IEnumerable<ExtractedProperty> GetProperties(ExtractedReferenceType namedType);

		/// <summary>
		/// Compiles properties from the queue of types
		/// </summary>
		/// <param name="namedTypes"></param>
		/// <returns></returns>
		protected IEnumerable<ExtractedProperty> GetPropertiesFromTypes(Queue<INamedType> namedTypes)
		{
			Dictionary<string, IProperty> properties = new Dictionary<string, IProperty>();

			// Add their properties
			while (namedTypes.Count > 0)
			{
				INamedType nonExtrClass = namedTypes.Dequeue();
				foreach (IProperty property in nonExtrClass.Properties)
				{
					if (!properties.ContainsKey(property.Name))
					{
						properties.Add(property.Name, property);
					}
				}
			}

			return properties.Values.Select(prp => TypeFactory.CreateExtractedProperty(prp));
		}
	}

	/// <summary>
	/// A property retrieve strategy for classes
	/// </summary>
	public class ClassPropertyRetrieveStrategy : PropertyRetrieveStrategy
	{

		internal ClassPropertyRetrieveStrategy(TypeFactory typeTable) : base(typeTable)
		{
		}

		/// <summary>
		/// Gets the properties
		/// </summary>
		/// <param name="namedType"></param>
		/// <returns></returns>
		public override IEnumerable<ExtractedProperty> GetProperties(ExtractedReferenceType namedType)
		{
			// Get all base types that aren't extracted
			Queue<INamedType> typesToExtract = GetTypesInHierarchyToExtract(namedType.NamedType);
			return GetPropertiesFromTypes(typesToExtract);
		}

		/// <summary>
		/// Gets a queue of all non-extracted base types up to the first extracted base type
		/// </summary>
		/// <param name="cl">The <see cref="INamedType"/> to use</param>
		/// <returns>A queue of all non-extracted base types</returns>
		private Queue<INamedType> GetTypesInHierarchyToExtract(INamedType cl)
		{
			Queue<INamedType> nonExtractedClasses = new Queue<INamedType>();
			nonExtractedClasses.Enqueue(cl);
			while (cl.BaseType != null)
			{
				if (TypeFactory.ContainsNamedType(cl.BaseType))
				{
					return nonExtractedClasses;
				}
				else
				{
					nonExtractedClasses.Enqueue(cl.BaseType);
					cl = cl.BaseType;
				}
			}

			return nonExtractedClasses;
		}
	}

	/// <summary>
	/// A property retrieve strategy for interfaces
	/// </summary>
	public class InterfacePropertyRetrieveStrategy : PropertyRetrieveStrategy
	{
		internal InterfacePropertyRetrieveStrategy(TypeFactory typeTable) : base(typeTable)
		{
		}

		/// <summary>
		/// Gest the properties
		/// </summary>
		/// <param name="namedType"></param>
		/// <returns></returns>
		public override IEnumerable<ExtractedProperty> GetProperties(ExtractedReferenceType namedType)
		{
			// Get all base types that aren't extracted
			Queue<INamedType> typesToExtract = GetInterfacesToExtract(namedType.NamedType);
			return GetPropertiesFromTypes(typesToExtract);
		}

		private Queue<INamedType> GetInterfacesToExtract(INamedType namedType)
		{
			Queue<INamedType> typeQueue = new Queue<INamedType>();
			typeQueue.Enqueue(namedType);

			GetInterfacesToExtractCore(namedType, typeQueue);
			return typeQueue;
		}

		private void GetInterfacesToExtractCore(INamedType namedType, Queue<INamedType> typeQueue)
		{
			foreach (INamedType baseInterface in namedType.Interfaces)
			{
				if (!TypeFactory.ContainsNamedType(baseInterface))
				{
					typeQueue.Enqueue(baseInterface);
					GetInterfacesToExtractCore(baseInterface, typeQueue);
				}
			}
		}
	}
}
