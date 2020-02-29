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

	public class CommandResult
	{
		public bool Success { get; set; }

		public string ErrorMessage { get; set; }
	}

	public class CommandResult<T> : CommandResult
	{
		public T Result { get; set; }
	}
}
