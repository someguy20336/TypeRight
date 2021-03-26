using NetStandardLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
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

		[JsonPropertyName("overrideSysText")]
        public int SystemTextJsonOverride { get; set; }

		[JsonProperty(PropertyName = "overrideNewtonsoft")]
        public int NewtonsoftOverride { get; set; }

		[JsonProperty(Order = 1)]
		public int NoNewtonsoftOverride { get; set; }
	}

	[ScriptObject]
	public class ASimpleModel<T>
	{
		public T GenericThing { get; set; }
	}

	public class ASimpleModel<T1, T2>
	{
		public ASimpleModel<T1> Reference { get; set; }

		public ASimpleEnum AhDoesThisWork { get; set; }

		public ASimpleModel<T1>[] ReferenceArray { get; set; }


		public List<ASimpleModel<T1>> ReferenceList { get; set; }

		public Dictionary<int, NetStandardEnum> EnumDict { get; set; }

		public NetStandardEnum? NullableEnum { get; set; }
	}

	public enum ASimpleEnum
	{
		A, 
		B,
		C
	}
}
