
using TypeRight.CodeModel;
using System.Collections.Generic;
using System.Linq;

namespace TypeRight.Packages
{
	/// <summary>
	/// Represents a package of found types that will be extracted into scripts
	/// </summary>
	public class ScriptPackage
    {
		/// <summary>
		/// Gets the list of named types being extracted
		/// </summary>
		public ScriptItemCollection<INamedType> NamedTypes { get; } = new ScriptItemCollection<INamedType>();

		/// <summary>
		/// Gets a list of MVC controllers being extracted
		/// </summary>
		public ScriptItemCollection<INamedType> MvcControllers { get; } = new ScriptItemCollection<INamedType>();



		/// <summary>
		/// Creates a new <see cref="ScriptPackage"/>
		/// </summary>
		private ScriptPackage()
        {
        }

		/// <summary>
		/// Builds a package using the given type iterator and visitor
		/// </summary>
		/// <param name="iterator">The type iterator</param>
		/// <param name="visitor">The type visitor</param>
		/// <returns>The package</returns>
		public static ScriptPackage BuildPackage(ITypeIterator iterator, TypeVisitor visitor)
		{
			iterator.IterateTypes(visitor);
			ScriptPackage package = new ScriptPackage();
			package.AddClassRange(visitor.FoundTypes);
			package.AddControllerRange(visitor.FoundControllers);
			return package;
		}

		/// <summary>
		/// Adds a range of <see cref="INamedType"/>
		/// </summary>
		/// <param name="classList">The <see cref="INamedType"/> to add</param>
		private void AddClassRange(IEnumerable<INamedType> classList)
		{
			foreach (INamedType oneClass in classList)
			{
				AddClass(oneClass);
			}
		}

		/// <summary>
		/// Adds a class to the package
		/// </summary>
		/// <param name="cl">the <see cref="INamedType"/> add</param>
		private void AddClass(INamedType cl)
        {
			if (HasClass(cl.FullName))
			{
				return;
			}
			NamedTypes.Add(cl);
        }

        

        /// <summary>
        /// Gets whether the class with the given name exists
        /// </summary>
        /// <param name="fullName">The full name of the class</param>
        /// <returns>True if it exists</returns>
        public bool HasClass(string fullName)
        {
            return NamedTypes.ContainsItemWithName(fullName);
        }
		
		/// <summary>
		/// Adds the given controller to the package
		/// </summary>
		/// <param name="controller">The controller</param>
		private void AddController(INamedType controller)
		{
			MvcControllers.Add(controller);
		}

		/// <summary>
		/// Adds a range of <see cref="INamedType"/>
		/// </summary>
		/// <param name="controllerList">The range of <see cref="INamedType"/></param>
		private void AddControllerRange(IEnumerable<INamedType> controllerList)
		{
			foreach (INamedType oneController in controllerList)
			{
				AddController(oneController);
			}
		}
	}
}
