
#nullable enable
using System.Collections.Generic;
using TypeRight.Attributes;

namespace TestAspNetCoreApp.Models;

[ScriptObject]
public class ANullableModel
{
	/// <summary>
	/// int?
	/// </summary>
    public int? PropOne { get; set; }

	/// <summary>
	/// string?
	/// </summary>
    public string? PropTwo { get; set; }

	/// <summary>
	/// ASimpleEnum?
	/// </summary>
    public ASimpleEnum? NullableEnum { get; set; }

	/// <summary>
	/// ASimpleModel?
	/// </summary>
	public ASimpleModel? NullableRefType { get; set; }

	/// <summary>
	///  List[int]?
	/// </summary>
	public List<int>? NullableList { get; set; }
	/// <summary>
	///  List[int?]?
	/// </summary>
	public List<int?>? NullableListNullElement { get; set; }
	/// <summary>
	/// int[]?
	/// </summary>
	public int[]? NullableArray { get; set; }
	/// <summary>
	/// int?[]?
	/// </summary>
	public int?[]? NullableArrayNullElement { get; set; }
	/// <summary>
	/// ASimpleModel[]?
	/// </summary>
	public ASimpleModel[]? NullableArrayRefTypeElement { get; set; }
	/// <summary>
	/// Dictionary[int, ASimpleModel?]
	/// </summary>
	public Dictionary<int, ASimpleModel?> DictionaryNullValue { get; set; } = null!;
}