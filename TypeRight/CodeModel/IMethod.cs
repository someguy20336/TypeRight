using System.Collections.Generic;

namespace TypeRight.CodeModel
{
	/// <summary>
	/// Represents a type method
	/// </summary>
	public interface IMethod
	{
		/// <summary>
		/// Gets the name of the method
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Gets the summary comments for the method
		/// </summary>
		string SummaryComments { get; }
		
		/// <summary>
		/// Gets the returns comments for the method
		/// </summary>
		string ReturnsComments { get; }

		/// <summary>
		/// Gets the return type of the action
		/// </summary>
		IType ReturnType { get; }

		/// <summary>
		/// Gets the parameters for the method
		/// </summary>
		IReadOnlyList<IMethodParameter> Parameters { get; }

		/// <summary>
		/// Gets the attributes for the method
		/// </summary>
		IReadOnlyList<IAttributeData> Attributes { get; }
	}
}
