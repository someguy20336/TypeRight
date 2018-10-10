using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TypeRight.Attributes;

namespace TestAspNetCoreApp.Models
{
	[ScriptObject]
    public class ASimpleModel
    {
		public int PropOne { get; set; }

		public string PropTwo { get; set; }
	}
}
