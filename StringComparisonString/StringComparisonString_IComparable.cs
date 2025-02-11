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
	public int CompareTo(object? obj)
	{
		// https://msdn.microsoft.com/en-us/library/4d7sx9hd(v=vs.110).aspx
		if (obj is null)
		{
			return 1; // If other is not a valid object reference, this instance is greater.
		}

		// obj must be either StringOrdinalIgnoreCase or String
		if (obj is not StringComparisonString<TStringComparison> other)
		{
			var s_other = obj as string ?? throw new ArgumentException("Object must be of type " + nameof(StringComparisonString<TStringComparison>) + " or String.");
			return CompareTo(s_other); // call CompareTo(string)
		}

		return CompareTo(other); // call CompareTo(StringOrdinalIgnoreCase)
	}
	public int CompareTo(StringComparisonString<TStringComparison>? other)
	{
		// https://msdn.microsoft.com/en-us/library/4d7sx9hd(v=vs.110).aspx
		if (other is null)
		{
			return 1; // If other is not a valid object reference, this instance is greater.
		}

		if (ReferenceEquals(Value, other.Value))
		{
			return 0;
		}

		return CompareTo(other.Value); // call CompareTo(string)
	}
	public int CompareTo(string? other)
	{
		// https://msdn.microsoft.com/en-us/library/4d7sx9hd(v=vs.110).aspx
		if (other is null)
		{
			return 1; // If other is not a valid object reference, this instance is greater.
		}

		return comparer_.Compare(Value, other);
	}

	public static bool operator <(StringComparisonString<TStringComparison> left, StringComparisonString<TStringComparison> right)
	{
		return left is null ? right is not null : left.CompareTo(right) < 0;
	}
	public static bool operator <(StringComparisonString<TStringComparison> left, string right)
	{
		return left is null ? right is not null : left.CompareTo(right) < 0;
	}
	public static bool operator <(string left, StringComparisonString<TStringComparison> right)
	{
		return left is null ? right is not null : !(right >= left);
	}

	public static bool operator <=(StringComparisonString<TStringComparison> left, StringComparisonString<TStringComparison> right)
	{
		return left is null || left.CompareTo(right) <= 0;
	}
	public static bool operator <=(StringComparisonString<TStringComparison> left, string right)
	{
		return left is null || left.CompareTo(right) <= 0;
	}
	public static bool operator <=(string left, StringComparisonString<TStringComparison> right)
	{
		return left is null || !(right > left);
	}

	public static bool operator >(StringComparisonString<TStringComparison> left, StringComparisonString<TStringComparison> right)
	{
		return left is not null && left.CompareTo(right) > 0;
	}
	public static bool operator >(StringComparisonString<TStringComparison> left, string right)
	{
		return left is not null && left.CompareTo(right) > 0;
	}
	public static bool operator >(string left, StringComparisonString<TStringComparison> right)
	{
		return left is not null && !(left <= right);
	}

	public static bool operator >=(StringComparisonString<TStringComparison> left, StringComparisonString<TStringComparison> right)
	{
		return left is null ? right is null : left.CompareTo(right) >= 0;
	}
	public static bool operator >=(StringComparisonString<TStringComparison> left, string right)
	{
		return left is null ? right is null : left.CompareTo(right) >= 0;
	}
	public static bool operator >=(string left, StringComparisonString<TStringComparison> right)
	{
		return left is null ? right is null : !(left < right);
	}
}
