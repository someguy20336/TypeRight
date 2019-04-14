using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestAspNetCoreApp.Models
{
	public class CustomGroupObject1
	{
		public int Prop1 { get; set; }

		public NetStandardLib.NetStandardClass OtherClass { get; set; }
	}

	public class CustomGroupObj2
	{
		public CustomGroupObject1 Obj1 { get; set; }
	}
}
