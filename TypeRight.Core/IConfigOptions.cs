using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeRight
{
	/// <summary>
	/// Basic configuration options
	/// </summary>
	public interface IConfigOptions
	{
		/// <summary>
		/// Gets or sets whether the generator is enabled
		/// </summary>
		bool Enabled { get; set; }

		/// <summary>
		/// Gets or sets the filepath to the server objects
		/// </summary>
		string ServerObjectsResultFilepath { get; set; }
	}
}
