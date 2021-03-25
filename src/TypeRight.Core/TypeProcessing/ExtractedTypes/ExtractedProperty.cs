using TypeRight.CodeModel;

namespace TypeRight.TypeProcessing
{
	/// <summary>
	/// A property that is extracted
	/// </summary>
	public class ExtractedProperty
	{
		/// <summary>
		/// Gets the type descriptor of this property
		/// </summary>
		public TypeDescriptor Type { get; }

		/// <summary>
		/// Gets the comments for the property
		/// </summary>
		public string Comments { get; }

		/// <summary>
		/// Gets the name of the property
		/// </summary>
		public string Name { get; }

		public string OutputName { get; set; }

		internal ExtractedProperty(IProperty property, TypeTable typeTable)
		{
			Name = property.Name;
			Comments = property.Comments;
			OutputName = typeTable.Settings.NamingStrategy.GetName(property);
			Type = typeTable.LookupType(property.PropertyType);  
		}
	}
}
