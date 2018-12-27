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
}
