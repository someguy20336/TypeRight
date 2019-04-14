using System;

namespace NetStandardLib
{
	/// <summary>
	/// Start some doc here
	/// </summary>
	public class NetStandardClass
	{
		public int HelloThere { get; set; }

		public NetStandardEnum EnumType { get; set; }
	}

	public enum NetStandardEnum
	{
		One,
		Two,
		Three
	}
}
