
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

	/// <summary>
	/// AGenericTypeHere[int]
	/// </summary>
	public AGenericTypeHere<int> GenericNotNull { get; set; } = null!;

	/// <summary>
	/// AGenericTypeHere[int?]
	/// </summary>
	public AGenericTypeHere<int?> GenericNullable { get; set; } = null!;
}

[ScriptObject]
public class AGenericTypeHere<T>
{
	public T NotNull { get; set; } = default!;
	public T? Nullable { get; set; }
}