namespace TypeRight.CodeModel.Default
{
	public class ArrayType : IArrayType
	{
		public IType ElementType { get; }

		public string Name { get; }

		public bool IsNullable => false;

		public ArrayType(IType elementType, string name)
		{
			ElementType = elementType;
			Name = name;
		}

		public IType AsNonNullable() => this;
	}
}
