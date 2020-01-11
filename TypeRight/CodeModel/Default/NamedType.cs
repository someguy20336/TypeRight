using System;
using System.Collections.Generic;
using System.Text;

namespace TypeRight.CodeModel.Default
{
	public class NamedType : INamedType
	{
		public INamedType ConstructedFromType { get; }

		public INamedType BaseType { get; }

		public IReadOnlyList<INamedType> Interfaces { get; }

		public IReadOnlyList<IType> TypeArguments { get; }

		public string Comments { get; }

		public IReadOnlyList<IProperty> Properties { get; }

		public IReadOnlyList<IField> Fields { get; }

		public IReadOnlyList<IMethod> Methods { get; }

		public IReadOnlyList<IAttributeData> Attributes { get; }

		public TypeFlags Flags { get; }

		public string FilePath { get; }

		public string Name { get; }

		public string FullName { get; }
		// TODO: factory for specifc types
		public NamedType(string name, 
			string fullName,
			TypeFlags flags = null,
			IReadOnlyList<IProperty> properties = null, 
			INamedType baseType = null, 
			IReadOnlyList<INamedType> interfaces = null, 
			string filePath = null,
			INamedType constructedFromType = null,
			IReadOnlyList<IType> typeArguments = null, 
			IReadOnlyList<IField> fields = null, 
			IReadOnlyList<IMethod> methods = null, 
			IReadOnlyList<IAttributeData> attributes = null, 
			string comments = "")
		{
			Name = name;
			FullName = fullName;
			Properties = properties ?? new List<Property>();
			BaseType = baseType;
			ConstructedFromType = constructedFromType ?? this;
			Interfaces = interfaces ?? new List<NamedType>();
			FilePath = filePath;
			TypeArguments = typeArguments ?? new List<TypeBase>();
			Fields = fields;
			Methods = methods;
			Attributes = attributes;
			Flags = flags ?? new TypeFlags();
			Comments = comments;
		}
	}
}
