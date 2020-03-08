using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeRight.CodeModel;

namespace TypeRight.Tests.TestBuilders.TypeCollection
{
	internal interface IAttributable
	{
		TypeCollectionBuilder TypeCollectionBuilder { get; }
		List<IAttributeData> Attributes { get; }
	}
}
