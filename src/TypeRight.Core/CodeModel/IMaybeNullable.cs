namespace TypeRight.CodeModel;

public interface IMaybeNullable : IType
{
	bool IsNullable { get; }

	IType AsNonNullable();
}
