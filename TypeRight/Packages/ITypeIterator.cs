using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeRight.Packages
{
	/// <summary>
	/// An object that iterates types with the given visitor
	/// </summary>
	public interface ITypeIterator
	{
		/// <summary>
		/// Iterates types
		/// </summary>
		/// <param name="visitor">The visitor to use when a type is found</param>
		void IterateTypes(TypeVisitor visitor);
	}
}
