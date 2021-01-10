using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeRight.CodeModel
{
	/// <summary>
	/// An array type
	/// </summary>
	public interface IArrayType : IType
	{
		/// <summary>
		/// The element type of the array
		/// </summary>
		IType ElementType { get; }
	}
}
