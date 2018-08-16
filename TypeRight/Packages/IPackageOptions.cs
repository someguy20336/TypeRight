using TypeRight.Configuration;

namespace TypeRight.Packages
{
	/// <summary>
	/// Options used when creating packages
	/// </summary>
	public interface IPackageOptions
	{
		/// <summary>
		/// Gets the default class namespace
		/// </summary>
		string ClassNamespace { get; }

		/// <summary>
		/// Gets the default enum namespace
		/// </summary>
		string EnumNamespace { get; }

		/// <summary>
		/// Gets the default web methods namespace
		/// </summary>
		string WebMethodNamespace { get; }

		/// <summary>
		/// Gets the MVC action attribute name
		/// </summary>
		string MvcActionAttributeName { get; }
	}
}
