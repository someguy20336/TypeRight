using TypeRight.Attributes;
using TypeRight.CodeModel;
using TypeRight.TypeFilters;
using System.Collections.Generic;
using System.Linq;

namespace TypeRight.TypeProcessing
{
	/// <summary>
	/// Represents an extracted enum type
	/// </summary>
	public class ExtractedEnumType : ExtractedType
	{
		/// <summary>
		/// Gets whether or not this enum type should use the extended syntax
		/// </summary>
		public bool UseExtendedSyntax { get; }

		/// <summary>
		/// Gets the members of this enum
		/// </summary>
		public IReadOnlyList<EnumMemberInfo> Members { get; }

		internal ExtractedEnumType(INamedType namedType, string typeNamespace, TypeFilter dispNameFilter) : base(namedType, typeNamespace)
		{
			// Use extended
			string attributeName = typeof(ScriptEnumAttribute).FullName;
			string useExtendedName = nameof(ScriptEnumAttribute.UseExtendedSyntax);
			foreach (IAttributeData attrData in namedType.Attributes)
			{
				if (attrData.AttributeType.FullName == attributeName)
				{
					if (attrData.NamedArguments.ContainsKey(useExtendedName))
					{
						UseExtendedSyntax = (bool)attrData.NamedArguments[useExtendedName];
					}
				}
			}

			// Members
			Members = namedType.Fields.Select(field => new EnumMemberInfo(field, dispNameFilter)).ToList().AsReadOnly();
		}
	}
}
