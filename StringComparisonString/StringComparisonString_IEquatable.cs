using System;

#nullable enable

namespace JDanielSmith.System;

/// <summary>
/// Provide a wrapper around System.String which automatically uses TStringComparison instead of having
/// to explicitly specify a System.StringComparison value.
/// 
/// This is especially useful when using strings as keys in collections, where the key is something like a Windows file-system pathname;
/// it can be easy to forget to pass an IEqualityComparer<> in the constructor.
/// 
/// Some hints from: http://stackoverflow.com/questions/33039324/how-can-system-string-be-properly-wrapped-for-case-insensitivy
/// </summary>
public sealed partial class StringComparisonString<TStringComparison>
{
	public override bool Equals(object? obj)
	{
		if (obj is null)
			return false; // this != null

		var other = obj as StringComparisonString<TStringComparison>;
		if (other is not null)
			return Equals(other); // call Equals(StringComparisonString<TStringComparison>)

		var s_other = obj as string;
		if (s_other is not null)
			return Equals(s_other); // call Equals(string)

		return comparer_.Equals(obj);
	}
	public bool Equals(StringComparisonString<TStringComparison>? other)
	{
		if (other is null)
			return false; // this != null
		return Equals(other.Value); // call Equals(string)
	}
	public bool Equals(string? other) => comparer_.Equals(Value, other);
	public override int GetHashCode() => comparer_.GetHashCode(Value);

	public static bool operator ==(StringComparisonString<TStringComparison> x, StringComparisonString<TStringComparison> y)
	{
		if (x is null)
			return (y is null); // null == null, null != something
		return x.Equals(y); // know x != null
	}
	public static bool operator ==(StringComparisonString<TStringComparison> x, string y)
	{
		if (x is null)
			return (y is null); // null == null, null != something
		return x.Equals(y); // know x != null
	}
	public static bool operator ==(string x, StringComparisonString<TStringComparison> y) => y == x; // == is commutative, x == y
	public static bool operator !=(StringComparisonString<TStringComparison> x, StringComparisonString<TStringComparison> y) => !(x == y);
	public static bool operator !=(string x, StringComparisonString<TStringComparison> y) => !(x == y);
	public static bool operator !=(StringComparisonString<TStringComparison> x, string y) => !(x == y);
}

