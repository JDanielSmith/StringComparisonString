using System;

using CodeAnalysis = System.Diagnostics.CodeAnalysis;

namespace JDanielSmith.System
{
	/// <summary>
	/// Provide a case-insensitive wrapper around System.String.
	/// 
	/// This is especially useful when using strings as keys in collections, where the key is something like a Windows file-system pathname;
	/// it can be easy to forget to pass an IEqualityComparer<> in the constructor.
	/// 
	/// Some hints from: http://stackoverflow.com/questions/33039324/how-can-system-string-be-properly-wrapped-for-case-insensitivy
	/// </summary>
	[CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "String")]
	[CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes")]
	public sealed class String<TComparison> : IComparable, ICloneable,
		IComparable<String<TComparison>>, IEquatable<String<TComparison>>,
		IComparable<String>, IEquatable<String>
		where TComparison : StringComparison, new()
	{
		static readonly StringComparison _comparison = new TComparison();
		static readonly global::System.StringComparison _comparisonType = _comparison.Comparison;
		static readonly StringComparer _comparer = StringComparer.FromComparison(_comparisonType);

		public string Value { get; }

		public String(string value)
		{
			// matching the behavior of System.String is more straight-forward if "Value" is never null
			Value = value ?? String.Empty;
		}

		// easily convert to/from System.String
		public static implicit operator String<TComparison>(string source) => new String<TComparison>(source);
		public static implicit operator string(String<TComparison> source) => source?.Value;

		#region Equals, IEquatable
		public override bool Equals(object obj)
		{
			if (obj is null)
				return false; // this != null

			var other = obj as String<TComparison>;
			if (!(other is null))
				return Equals(other); // call Equals(String<TStringComparerAndComparison>)

			var s_other = obj as string;
			if (!(s_other is null))
				return Equals(s_other); // call Equals(string)

			return _comparer.Equals(obj);
		}
		public bool Equals(String<TComparison> other)
		{
			if (other is null)
				return false; // this != null
			return Equals(other.Value); // call Equals(string)
		}
		public bool Equals(string other) => _comparer.Equals(Value, other);
		public override int GetHashCode() => _comparer.GetHashCode(Value); 
		#endregion

		public override string ToString() => Value;

		public object Clone() => new String<TComparison>(Value);

		#region IComparable
		public int CompareTo(object obj)
		{
			// https://msdn.microsoft.com/en-us/library/4d7sx9hd(v=vs.110).aspx
			if (obj is null)
				return 1; // If other is not a valid object reference, this instance is greater.

			// obj must be either StringOrdinalIgnoreCase or String
			var other = obj as String<TComparison>;
			if (other is null)
			{
				var s_other = obj as string;
				if (s_other is null)
					throw new ArgumentException("Object must be of type " + nameof(String<TComparison>) + " or String.");

				return CompareTo(s_other); // call CompareTo(string)
			}

			return CompareTo(other); // call CompareTo(StringOrdinalIgnoreCase)
		}
		public int CompareTo(String<TComparison> other)
		{
			// https://msdn.microsoft.com/en-us/library/4d7sx9hd(v=vs.110).aspx
			if (other is null)
				return 1; // If other is not a valid object reference, this instance is greater.

			if (Object.ReferenceEquals(Value, other.Value))
				return 0;

			return CompareTo(other.Value); // call CompareTo(string)
		}
		public int CompareTo(string other)
		{
			// https://msdn.microsoft.com/en-us/library/4d7sx9hd(v=vs.110).aspx
			if (other is null)
				return 1; // If other is not a valid object reference, this instance is greater.

			return _comparer.Compare(Value, other);
		}

		public static bool operator ==(String<TComparison> x, String<TComparison> y)
		{
			if (x is null)
				return (y is null); // null == null, null != something
			return x.Equals(y); // know x != null
		}
		public static bool operator ==(String<TComparison> x, string y)
		{
			if (x is null)
				return (y is null); // null == null, null != something
			return x.Equals(y); // know x != null
		}
		public static bool operator ==(string x, String<TComparison> y) => y == x; // == is commutative, x == y
		public static bool operator !=(String<TComparison> x, String<TComparison> y) => !(x == y);
		public static bool operator !=(string x, String<TComparison> y) => !(x == y);
		public static bool operator !=(String<TComparison> x, string y) => !(x == y);
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
