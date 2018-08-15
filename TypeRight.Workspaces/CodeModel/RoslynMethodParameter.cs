using TypeRight.CodeModel;

namespace TypeRight.Workspaces.CodeModel
{
	class RoslynMethodParameter : IMethodParameter
	{
		public string Name { get; }

		public string Comments { get; }

		public IType ParameterType { get; }

		public RoslynMethodParameter(string name, string comments, IType type)
		{
			Name = name;
			Comments = comments;
			ParameterType = type;
		}
	}
}
