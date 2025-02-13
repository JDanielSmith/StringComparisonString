﻿namespace JDanielSmith.System;

/// <summary>
/// Provide a wrapper around System.String which automatically uses TStringComparison instead of having
/// to explicitly specify a System.StringComparison value.
/// 
/// This is especially useful when using strings as keys in collections, where the key is something like a Windows file-system pathname;
/// it can be easy to forget to pass an IEqualityComparer<> in the constructor.
/// 
/// Some hints from: http://stackoverflow.com/questions/33039324/how-can-system-string-be-properly-wrapped-for-case-insensitivy
/// </summary>
public sealed partial class StringComparisonString<TStringComparison>(string value) :
	ICloneable,
	IEquatable<String?>, IEquatable<StringComparisonString<TStringComparison>?>,
	IComparable, IComparable<String?>, IComparable<StringComparisonString<TStringComparison>?>
where TStringComparison : StringComparison.IStringComparison
{
	static readonly global::System.StringComparison comparison_ = TStringComparison.Comparison;
	static readonly StringComparer comparer_ = StringComparer.FromComparison(comparison_);

	public string Value { get; } = value ?? String.Empty;

	public override string ToString()
	{
		return Value;
	}

	public char this[int index] => Value[index];
	public int Length => Value.Length;

	public object Clone()
	{
		return new StringComparisonString<TStringComparison>(Value);
	}

	// easily convert to/from System.String
	public static implicit operator StringComparisonString<TStringComparison>(string source)
	{
		return new(source);
	}

	public StringComparisonString<TStringComparison> FromString(string source)
	{
		return new(source);
	}

	public static implicit operator string?(StringComparisonString<TStringComparison>? source)
	{
		return source?.Value;
	}

	public bool Contains(String value)
	{
		return Value.Contains(value, comparison_);
	}

	public bool Contains(char value)
	{
		return Value.Contains(value, comparison_);
	}

	public bool EndsWith(string value)
	{
		return Value.EndsWith(value, comparison_);
	}

	public int IndexOf(string value)
	{
		return Value.IndexOf(value, comparison_);
	}

	public int IndexOf(string value, int startIndex)
	{
		return Value.IndexOf(value, startIndex, comparison_);
	}

	public int IndexOf(string value, int startIndex, int count)
	{
		return Value.IndexOf(value, startIndex, count, comparison_);
	}

	public int IndexOf(char value)
	{
		return Value.IndexOf(value, comparison_);
	}

	public int LastIndexOf(string value)
	{
		return Value.LastIndexOf(value, comparison_);
	}

	public int LastIndexOf(string value, int startIndex)
	{
		return Value.LastIndexOf(value, startIndex, comparison_);
	}

	public int LastIndexOf(string value, int startIndex, int count)
	{
		return Value.LastIndexOf(value, startIndex, count, comparison_);
	}

	public string Replace(string oldValue, string? newValue)
	{
		return Value.Replace(oldValue, newValue, comparison_);
	}

	public bool StartsWith(string value)
	{
		return Value.StartsWith(value, comparison_);
	}
}

public static class StringComparisonString
{
	public static StringComparisonString<TStringComparison> FromString<TStringComparison>(string source) where TStringComparison : StringComparison.IStringComparison, new()
	{
		return new StringComparisonString<TStringComparison>(source);
	}

	public static int Compare<TStringComparison>(StringComparisonString<TStringComparison>? strA, int indexA, StringComparisonString<TStringComparison>? strB, int indexB, int length) where TStringComparison : StringComparison.IStringComparison, new()
	{
		return String.Compare(strA?.Value, indexA, strB?.Value, indexB, length, TStringComparison.Comparison);
	}
	public static int Compare<TStringComparison>(StringComparisonString<TStringComparison>? strA, int indexA, string? strB, int indexB, int length) where TStringComparison : StringComparison.IStringComparison, new()
	{
		return String.Compare(strA?.Value, indexA, strB, indexB, length, TStringComparison.Comparison);
	}
	public static int Compare<TStringComparison>(string? strA, int indexA, StringComparisonString<TStringComparison>? strB, int indexB, int length) where TStringComparison : StringComparison.IStringComparison, new()
	{
		return String.Compare(strA, indexA, strB?.Value, indexB, length, TStringComparison.Comparison);
	}

	public static int Compare<TStringComparison>(StringComparisonString<TStringComparison>? strA, StringComparisonString<TStringComparison>? strB) where TStringComparison : StringComparison.IStringComparison, new()
	{
		if (strA is null)
		{
			return strB is null ? 0 : -1; // If strB is a valid object reference, that instance is greater.
		}
		return strA.CompareTo(strB);
	}
	public static int Compare<TStringComparison>(StringComparisonString<TStringComparison>? strA, string? strB) where TStringComparison : StringComparison.IStringComparison, new()
	{
		if (strA is null)
		{
			return strB is null ? 0 : -1; // If strB is a valid object reference, that instance is greater.
		}
		return strA.CompareTo(strB);
	}
	public static int Compare<TStringComparison>(string? strA, StringComparisonString<TStringComparison>? strB) where TStringComparison : StringComparison.IStringComparison, new()
	{
		return -1 * Compare(strB, strA);
	}

	public static bool Equals<TStringComparison>(StringComparisonString<TStringComparison>? a, StringComparisonString<TStringComparison>? b) where TStringComparison : StringComparison.IStringComparison, new()
	{
		if (a is null)
		{
			return b is null; // Equals(null, null) == true
		}
		return b is not null && a.Equals(b);
	}
	public static bool Equals<TStringComparison>(StringComparisonString<TStringComparison>? a, string? b) where TStringComparison : StringComparison.IStringComparison, new()
	{
		if (a is null)
		{
			return b is null; // Equals(null, null) == true
		}
		return b is not null && a.Equals(b);
	}
	public static bool Equals<TStringComparison>(string? a, StringComparisonString<TStringComparison>? b) where TStringComparison : StringComparison.IStringComparison, new()
	{
		return Equals(b, a);
	}

	public static int GetHashCode<TStringComparison>(ReadOnlySpan<char> value) where TStringComparison : StringComparison.IStringComparison, new()
	{
		return String.GetHashCode(value, TStringComparison.Comparison);
	}
}
