using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeRightTests.TestBuilders
{
	interface IAttributable
	{
		List<AttributeInfo> Attributes { get; }
	}
}
