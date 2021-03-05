using System.Collections.Generic;
using TypeRight.CodeModel;

namespace TypeRight.Tests.TestBuilders.TypeCollection
{
	public interface IAttributable
	{
		TypeCollectionBuilder TypeCollectionBuilder { get; }
		List<IAttributeData> Attributes { get; }
	}
}
