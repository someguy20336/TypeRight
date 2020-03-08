using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestAspNetCoreApp.Models
{
    public class GenericModel<T>
    {
		public T Prop1 { get; set; }
	}

	public class TestTwoTypeParams<T, T2>
	{
		public T Prop1 { get; set; }

		public T2 Prop2 { get; set; }
	}
}
