using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeRight.CodeModel;
using TypeRight.CodeModel.Default;

namespace TypeRight.Tests.TestBuilders
{
	internal interface IBuilderWithTypeNameProperties
	{
		List<SymbolInfo> Properties { get; }
	}

	internal interface IBuilderWithPropertyList
	{
		List<IProperty> Properties { get; }
	}
}
