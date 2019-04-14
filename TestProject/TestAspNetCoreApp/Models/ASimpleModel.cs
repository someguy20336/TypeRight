using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TypeRight.Attributes;

namespace TestAspNetCoreApp.Models
{
	/// <summary>
	/// Tst model
	/// </summary>
	[ScriptObject]
    public class ASimpleModel
    {
		/// <summary>
		/// Doc for prop 1
		/// </summary>
		public int PropOne { get; set; }

		/// <summary>
		/// Doc for prop 2
		/// </summary>
		public string PropTwo { get; set; }
	}

	[ScriptObject]
	public class ASimpleModel<T>
	{
		public T GenericThing { get; set; }
	}

	public class ASimpleModel<T1, T2>
	{
		public ASimpleModel<T1> Reference { get; set; }

		public int AhDoesThisWork { get; set; }
	}
}
