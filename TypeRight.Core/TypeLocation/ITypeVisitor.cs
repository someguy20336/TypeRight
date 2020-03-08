using TypeRight.CodeModel;

namespace TypeRight.TypeLocation
{
	public interface ITypeVisitor
	{
		void Visit(INamedType namedType, string targetPath = null);
		void VisitExternalType(INamedType namedType, string targetPath = null);
	}
}