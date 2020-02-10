﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace JDanielSmith.System
{
	/// <summary>
	/// Provide a wrapper around System.String which automatically uses TStringComparison instead of having
	/// to explictly specify a System.StringComparison value.
	/// 
	/// This is especially useful when using strings as keys in collections, where the key is something like a Windows file-system pathname;
	/// it can be easy to forget to pass an IEqualityComparer<> in the constructor.
	/// 
	/// Some hints from: http://stackoverflow.com/questions/33039324/how-can-system-string-be-properly-wrapped-for-case-insensitivy
	/// </summary>
	public sealed class StringComparisonString<TStringComparison> :
		ICloneable,
		IEquatable<String>,  IEquatable<StringComparisonString<TStringComparison>>,
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
		public StringComparisonString(char[] value) : this (new String(value)) 	{	}
		public StringComparisonString(ReadOnlySpan<char> value) : this(new String(value)) { }
		public StringComparisonString(char c, int count) : this(new String(c, count)) { }
		public StringComparisonString(char[] value, int startIndex, int length) : this(new String(value, startIndex, length)) { }

		public override string ToString() => Value;

		public object Clone() => new StringComparisonString<TStringComparison>(Value);

		// easily convert to/from System.String
		public static implicit operator StringComparisonString<TStringComparison>(string source) => new StringComparisonString<TStringComparison>(source);
		public StringComparisonString<TStringComparison> ToStringComparisonString(string source) => new StringComparisonString<TStringComparison>(source);

		public static implicit operator string?(StringComparisonString<TStringComparison>? source) => source?.Value;

		#region Equals, IEquatable
		public override bool Equals(object? obj)
		{
			if (obj is null)
				return false; // this != null

			var other = obj as StringComparisonString<TStringComparison>;
			if (!(other is null))
				return Equals(other); // call Equals(StringComparisonString<TStringComparison>)

			var s_other = obj as string;
			if (!(s_other is null))
				return Equals(s_other); // call Equals(string)

			return _comparer.Equals(obj);
		}
		public bool Equals(StringComparisonString<TStringComparison> other)
		{
			if (other is null)
				return false; // this != null
			return Equals(other.Value); // call Equals(string)
		}
		public bool Equals(string other) => _comparer.Equals(Value, other);
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
			if (ReferenceEquals(obj, null))
				return 1; // If other is not a valid object reference, this instance is greater.

			// obj must be either StringOrdinalIgnoreCase or String
			var other = obj as StringComparisonString<TStringComparison>;
			if (ReferenceEquals(other, null))
			{
				var s_other = obj as string;
				if (ReferenceEquals(s_other, null))
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
			if (ReferenceEquals(other, null))
				return 1; // If other is not a valid object reference, this instance is greater.

			if (ReferenceEquals(Value, other.Value))
				return 0;

			return CompareTo(other.Value); // call CompareTo(string)
		}
		public int CompareTo(string? other)
		{
			// https://msdn.microsoft.com/en-us/library/4d7sx9hd(v=vs.110).aspx
			if (ReferenceEquals(other, null))
				return 1; // If other is not a valid object reference, this instance is greater.

			return _comparer.Compare(Value, other);
		}

		public static bool operator <(StringComparisonString<TStringComparison> left, StringComparisonString<TStringComparison> right)
		{
			return ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;
		}
		public static bool operator <(StringComparisonString<TStringComparison> left, string right)
		{
			return ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;
		}
		public static bool operator <(string left, StringComparisonString<TStringComparison> right)
		{
			return ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : new StringComparisonString<TStringComparison>(left) < right;
		}

		public static bool operator <=(StringComparisonString<TStringComparison> left, StringComparisonString<TStringComparison> right)
		{
			return ReferenceEquals(left, null) || left.CompareTo(right) <= 0;
		}
		public static bool operator <=(StringComparisonString<TStringComparison> left, string right)
		{
			return ReferenceEquals(left, null) || left.CompareTo(right) <= 0;
		}
		public static bool operator <=(string left, StringComparisonString<TStringComparison> right)
		{
			return ReferenceEquals(left, null) || new StringComparisonString<TStringComparison>(left) <= right;
		}

		public static bool operator >(StringComparisonString<TStringComparison> left, StringComparisonString<TStringComparison> right)
		{
			return !ReferenceEquals(left, null) && left.CompareTo(right) > 0;
		}
		public static bool operator >(StringComparisonString<TStringComparison> left, string right)
		{
			return !ReferenceEquals(left, null) && left.CompareTo(right) > 0;
		}
		public static bool operator >(string left, StringComparisonString<TStringComparison> right)
		{
			return !ReferenceEquals(left, null) && new StringComparisonString<TStringComparison>(left) > right;
		}

		public static bool operator >=(StringComparisonString<TStringComparison> left, StringComparisonString<TStringComparison> right)
		{
			return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;
		}
		public static bool operator >=(StringComparisonString<TStringComparison> left, string right)
		{
			return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;
		}
		public static bool operator >=(string left, StringComparisonString<TStringComparison> right)
		{
			return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : new StringComparisonString<TStringComparison>(left) >= right;
		}

		#endregion

		#region IndexOf, LastIndexOf, StartsWith, EndsWith
		public bool EndsWith(string value) => Value.EndsWith(value, _comparisonType);
		public int IndexOf(string value) => Value.IndexOf(value, _comparisonType);
		public int IndexOf(string value, int startIndex) => Value.IndexOf(value, startIndex, _comparisonType);
		public int IndexOf(string value, int startIndex, int count) => Value.IndexOf(value, startIndex, count, _comparisonType);
		public int LastIndexOf(string value) => Value.LastIndexOf(value, _comparisonType);
		public int LastIndexOf(string value, int startIndex) => Value.LastIndexOf(value, startIndex, _comparisonType);
		public int LastIndexOf(string value, int startIndex, int count) => Value.LastIndexOf(value, startIndex, count, _comparisonType);
		public bool StartsWith(string value) => Value.StartsWith(value, _comparisonType);
		#endregion
	}
}
