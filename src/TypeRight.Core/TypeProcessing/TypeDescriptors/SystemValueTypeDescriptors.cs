using TypeRight.CodeModel;
using TypeRight.ScriptWriting;

namespace TypeRight.TypeProcessing
{
	/// <summary>
	/// A string type descriptor
	/// </summary>
	public class StringTypeDescriptor : TypeDescriptor
	{
		internal StringTypeDescriptor(INamedType type) : base(type)
		{
		}

		/// <summary>
		/// Formats the type
		/// </summary>
		/// <param name="formatter">The type formatter</param>
		/// <returns>The formatter type</returns>
		public override string FormatType(TypeFormatter formatter)
		{
			return formatter.FormatStringType(this);
		}
	}

	/// <summary>
	/// A numeric type descriptor
	/// </summary>
	public class NumericTypeDescriptor : TypeDescriptor
	{
		internal NumericTypeDescriptor(INamedType type) : base(type)
		{
		}

		/// <summary>
		/// Formats the type
		/// </summary>
		/// <param name="formatter">The type formatter</param>
		/// <returns>The formatter type</returns>
		public override string FormatType(TypeFormatter formatter)
		{
			return formatter.FormatNumericType(this);
		}
	}

	/// <summary>
	/// A boolean type descriptor
	/// </summary>
	public class BooleanTypeDescriptor : TypeDescriptor
	{
		internal BooleanTypeDescriptor(INamedType type) : base(type)
		{
		}

		/// <summary>
		/// Formats the type
		/// </summary>
		/// <param name="formatter">The type formatter</param>
		/// <returns>The formatter type</returns>
		public override string FormatType(TypeFormatter formatter)
		{
			return formatter.FormatBooleanType(this);
		}
	}

	/// <summary>
	/// A date and/or time type descriptor
	/// </summary>
	public class DateTimeTypeDescriptor : TypeDescriptor
	{
		internal DateTimeTypeDescriptor(INamedType type) : base(type)
		{
		}

		/// <summary>
		/// Formats the type
		/// </summary>
		/// <param name="formatter">The type formatter</param>
		/// <returns>The formatter type</returns>
		public override string FormatType(TypeFormatter formatter)
		{
			return formatter.FormatDateTimeType(this);
		}
	}
}
