namespace TypeRight.CodeModel
{
	/// <summary>
	/// Represents a type that is a type parameter 
	/// </summary>
	public interface ITypeParameter : IType
	{
		public bool IsNullable { get; }
	}
}
