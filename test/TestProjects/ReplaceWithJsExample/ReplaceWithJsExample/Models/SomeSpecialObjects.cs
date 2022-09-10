using System.Collections.Generic;

namespace TestAspNetCoreApp.Models
{
    public class CustomGroupObject1
	{
		/// <summary>
		/// Add doc
		/// </summary>
		public int Prop1 { get; set; }


		/// <summary>
		/// Just has a string list
		/// </summary>
		public List<string> AnotherStringList { get; set; }
	}

	public class CustomGroupObj2
	{
		public CustomGroupObject1 Obj1 { get; set; }
	}

	public class CustomGroupObj2<T>
	{
		public int Property { get; set; }
	}

	public class CustomGroupObj3
	{
        public string StringProp { get; set; }
        public CustomGroupObject1 Obj1 { get; set; }

	}
}
