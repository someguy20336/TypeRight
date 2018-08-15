using TypeRight.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeRightTests.HelperClasses
{
	[AttributeUsage(AttributeTargets.Field)]
	class MyEnumDescription : Attribute, IEnumDisplayNameProvider
	{
		public string Abbreviation { get; set; }

		public string DisplayName { get; set; }

		public MyEnumDescription()
		{

		}

		public MyEnumDescription(string dispName)
			: this(dispName, dispName)
		{ }

		public MyEnumDescription(string dispName, string abbrev)
		{
			DisplayName = dispName;
			Abbreviation = abbrev;
		}
	}
}
