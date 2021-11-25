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
public sealed class StringComparisonString<TStringComparison> :
	ICloneable,
	IEquatable<String?>,  IEquatable<StringComparisonString<TStringComparison>?>,
	IComparable, IComparable<String?>, IComparable<StringComparisonString<TStringComparison>?>
where TStringComparison : StringComparison, new()
{
	static readonly global::System.StringComparison _comparisonType = new TStringComparison().Comparison;
	static readonly StringComparer _comparer = StringComparer.FromComparison(_comparisonType);

	public string Value { get; }
	public StringComparisonString(string value)
	{
		Value = value ?? String.Empty;
	}

	public override string ToString() => Value;
	public char this[int index] { get => Value[index]; }
	public int Length { get => Value.Length; }

	public object Clone() => new StringComparisonString<TStringComparison>(Value);

	// easily convert to/from System.String
	public static implicit operator StringComparisonString<TStringComparison>(string source) => new StringComparisonString<TStringComparison>(source);
	public StringComparisonString<TStringComparison> FromString(string source) => new StringComparisonString<TStringComparison>(source);
	public static implicit operator string?(StringComparisonString<TStringComparison>? source) => source?.Value;

	#region Equals, IEquatable
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

		return _comparer.Equals(obj);
	}
	public bool Equals(StringComparisonString<TStringComparison>? other)
	{
		if (other is null)
			return false; // this != null
		return Equals(other.Value); // call Equals(string)
	}
	public bool Equals(string? other) => _comparer.Equals(Value, other);
	public override int GetHashCode() => _comparer.GetHashCode(Value);

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
	#endregion

	#region IComparable
	public int CompareTo(object? obj)
	{
		// https://msdn.microsoft.com/en-us/library/4d7sx9hd(v=vs.110).aspx
		if (obj is null)
			return 1; // If other is not a valid object reference, this instance is greater.

		// obj must be either StringOrdinalIgnoreCase or String
		var other = obj as StringComparisonString<TStringComparison>;
		if (other is null)
		{
			var s_other = obj as string;
			if (s_other is null)
#pragma warning disable CA1303 // Do not pass literals as localized parameters
				throw new ArgumentException("Object must be of type " + nameof(StringComparisonString<TStringComparison>) + " or String.");
#pragma warning restore CA1303 // Do not pass literals as localized parameters

			return CompareTo(s_other); // call CompareTo(string)
		}

		return CompareTo(other); // call CompareTo(StringOrdinalIgnoreCase)
	}
	public int CompareTo(StringComparisonString<TStringComparison>? other)
	{
		// https://msdn.microsoft.com/en-us/library/4d7sx9hd(v=vs.110).aspx
		if (other is null)
			return 1; // If other is not a valid object reference, this instance is greater.

		if (ReferenceEquals(Value, other.Value))
			return 0;

		return CompareTo(other.Value); // call CompareTo(string)
	}
	public int CompareTo(string? other)
	{
		// https://msdn.microsoft.com/en-us/library/4d7sx9hd(v=vs.110).aspx
		if (other is null)
			return 1; // If other is not a valid object reference, this instance is greater.

		return _comparer.Compare(Value, other);
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
	#endregion

	#region Contains, EndsWith, IndexOf, LastIndexOf, Replace, StartsWith

	public bool Contains(String value) => Value.Contains(value, _comparisonType);
	public bool Contains(char value) => Value.Contains(value, _comparisonType);


	public bool EndsWith(string value) => Value.EndsWith(value, _comparisonType);
		
	public int IndexOf(string value) => Value.IndexOf(value, _comparisonType);
	public int IndexOf(string value, int startIndex) => Value.IndexOf(value, startIndex, _comparisonType);
	public int IndexOf(string value, int startIndex, int count) => Value.IndexOf(value, startIndex, count, _comparisonType);
	public int IndexOf(char value) => Value.IndexOf(value, _comparisonType);
		
	public int LastIndexOf(string value) => Value.LastIndexOf(value, _comparisonType);
	public int LastIndexOf(string value, int startIndex) => Value.LastIndexOf(value, startIndex, _comparisonType);
	public int LastIndexOf(string value, int startIndex, int count) => Value.LastIndexOf(value, startIndex, count, _comparisonType);

	public string Replace(string oldValue, string? newValue) => Value.Replace(oldValue, newValue, _comparisonType);

	public bool StartsWith(string value) => Value.StartsWith(value, _comparisonType);

	#endregion
}

public static class StringComparisonString
{
	public static StringComparisonString<TStringComparison> FromString<TStringComparison>(string source) where TStringComparison : StringComparison, new()
	{
		return new StringComparisonString<TStringComparison>(source);
	}

	public static int Compare<TStringComparison>(StringComparisonString<TStringComparison>? strA, int indexA, StringComparisonString<TStringComparison>? strB, int indexB, int length) where TStringComparison : StringComparison, new()
	{
		return String.Compare(strA?.Value, indexA, strB?.Value, indexB, length, new TStringComparison().Comparison);
	}
	public static int Compare<TStringComparison>(StringComparisonString<TStringComparison>? strA, int indexA, string? strB, int indexB, int length) where TStringComparison : StringComparison, new()
	{
		return String.Compare(strA?.Value, indexA, strB, indexB, length, new TStringComparison().Comparison);
	}
	public static int Compare<TStringComparison>(string? strA, int indexA, StringComparisonString<TStringComparison>? strB, int indexB, int length) where TStringComparison : StringComparison, new()
	{
		return String.Compare(strA, indexA, strB?.Value, indexB, length, new TStringComparison().Comparison);
	}

	public static int Compare<TStringComparison>(StringComparisonString<TStringComparison>? strA, StringComparisonString<TStringComparison>? strB) where TStringComparison : StringComparison, new()
	{
		if (strA is null)
		{
			return strB is null ? 0 : -1; // If strB is a valid object reference, that instance is greater.
		}
		return strA.CompareTo(strB);
	}
	public static int Compare<TStringComparison>(StringComparisonString<TStringComparison>? strA, string? strB) where TStringComparison : StringComparison, new()
	{
		if (strA is null)
		{
			return strB is null ? 0 : -1; // If strB is a valid object reference, that instance is greater.
		}
		return strA.CompareTo(strB);
	}
	public static int Compare<TStringComparison>(string? strA, StringComparisonString<TStringComparison>? strB) where TStringComparison : StringComparison, new()
	{
		return -1 * Compare(strB, strA);
	}

	public static bool Equals<TStringComparison>(StringComparisonString<TStringComparison>? a, StringComparisonString<TStringComparison>? b) where TStringComparison : StringComparison, new()
	{
		if (a is null)
		{
			return b is null; // Equals(null, null) == true
		}
		return b is null ? false : a.Equals(b);
	}
	public static bool Equals<TStringComparison>(StringComparisonString<TStringComparison>? a, string? b) where TStringComparison : StringComparison, new()
	{
		if (a is null)
		{
			return b is null; // Equals(null, null) == true
		}
		return b is null ? false : a.Equals(b);
	}
	public static bool Equals<TStringComparison>(string? a, StringComparisonString<TStringComparison>? b) where TStringComparison : StringComparison, new()
	{
		return Equals(b, a);
	}

	public static int GetHashCode<TStringComparison>(ReadOnlySpan<char> value) where TStringComparison : StringComparison, new()
	{
		return String.GetHashCode(value, new TStringComparison().Comparison);
	}
}
