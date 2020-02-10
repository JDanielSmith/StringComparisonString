using System;
using System.Collections.Generic;
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
		ICloneable
	//	IComparable, IComparable<String?>, IComparable<StringComparisonString<TStringComparison>?>,
	// IEquatable<String>,  IEquatable<StringComparisonString<TStringComparison>>,
	where TStringComparison : StringComparison, new()
	{
		//static readonly global::System.StringComparison _comparisonType = new TStringComparison().Comparison;
		//static readonly StringComparer _comparer = StringComparer.FromComparison(_comparisonType);

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
	}
}
