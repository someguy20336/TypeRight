namespace TypeRight.CodeModel.Default
{
	public class TypeParameter : ITypeParameter
	{
		public string Name { get; }

		public bool IsNullable => false;

		public TypeParameter(string name)
		{
			Name = name;
		}

		public IType AsNonNullable() => this;
	}
}
