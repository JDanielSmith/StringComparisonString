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
	public sealed class String<TComparerAndComparison> : IComparable, ICloneable,
		IComparable<String<TComparerAndComparison>>, IEquatable<String<TComparerAndComparison>>,
		IComparable<String>, IEquatable<String>
		where TComparerAndComparison : StringComparerAndComparison, new()
	{
		static readonly StringComparerAndComparison _comparerAndComparison = new TComparerAndComparison();
		static readonly StringComparer _comparer = _comparerAndComparison.Comparer;
		static readonly StringComparison _comparisonType = _comparerAndComparison.Comparison;

		public string Value { get; }

		public String(string value)
		{
			// matching the behavior of System.String is more straight-forward if "Value" is never null
			Value = value ?? String.Empty;
		}

		// easily convert to/from System.String
		public static implicit operator String<TComparerAndComparison>(string source)
		{
			return new String<TComparerAndComparison>(source);
		}
		public static implicit operator string(String<TComparerAndComparison> source)
		{
			return source?.Value;
		}

		#region Equals, IEquatable
		public override bool Equals(object obj)
		{
			if (Object.ReferenceEquals(obj, null))
				return false; // this != null

			var other = obj as String<TComparerAndComparison>;
			if (!Object.ReferenceEquals(other, null))
				return Equals(other); // call Equals(String<TStringComparerAndComparison>)

			var s_other = obj as string;
			if (!Object.ReferenceEquals(s_other, null))
				return Equals(s_other); // call Equals(string)

			return _comparer.Equals(obj);
		}
		public bool Equals(String<TComparerAndComparison> other)
		{
			if (Object.ReferenceEquals(other, null))
				return false; // this != null
			return Equals(other.Value); // call Equals(string)
		}
		public bool Equals(string other)
		{
			return _comparer.Equals(Value, other);
		}

		public override int GetHashCode()
		{
			return _comparer.GetHashCode(Value);
		}
		#endregion

		public override string ToString()
		{
			return Value;
		}

		public object Clone()
		{
			return new String<TComparerAndComparison>(Value);
		}

		#region IComparable
		public int CompareTo(object obj)
		{
			// https://msdn.microsoft.com/en-us/library/4d7sx9hd(v=vs.110).aspx
			if (Object.ReferenceEquals(obj, null))
				return 1; // If other is not a valid object reference, this instance is greater.

			// obj must be either StringOrdinalIgnoreCase or String
			var other = obj as String<TComparerAndComparison>;
			if (Object.ReferenceEquals(other, null))
			{
				var s_other = obj as string;
				if (Object.ReferenceEquals(s_other, null))
					throw new ArgumentException("Object must be of type " + nameof(String<TComparerAndComparison>) + " or String.");

				return CompareTo(s_other); // call CompareTo(string)
			}

			return CompareTo(other); // call CompareTo(StringOrdinalIgnoreCase)
		}
		public int CompareTo(String<TComparerAndComparison> other)
		{
			// https://msdn.microsoft.com/en-us/library/4d7sx9hd(v=vs.110).aspx
			if (Object.ReferenceEquals(other, null))
				return 1; // If other is not a valid object reference, this instance is greater.

			if (Object.ReferenceEquals(Value, other.Value))
				return 0;

			return CompareTo(other.Value); // call CompareTo(string)
		}
		public int CompareTo(string other)
		{
			// https://msdn.microsoft.com/en-us/library/4d7sx9hd(v=vs.110).aspx
			if (Object.ReferenceEquals(other, null))
				return 1; // If other is not a valid object reference, this instance is greater.

			return _comparer.Compare(Value, other);
		}

		public static bool operator ==(String<TComparerAndComparison> x, String<TComparerAndComparison> y)
		{
			if (Object.ReferenceEquals(x, null))
				return Object.ReferenceEquals(y, null); // null == null, null != something
			return x.Equals(y); // know x != null
		}
		public static bool operator ==(String<TComparerAndComparison> x, string y)
		{
			if (Object.ReferenceEquals(x, null))
				return Object.ReferenceEquals(y, null); // null == null, null != something
			return x.Equals(y); // know x != null
		}
		public static bool operator ==(string x, String<TComparerAndComparison> y)
		{
			return y == x; // == is commutative, x == y
		}

		public static bool operator !=(String<TComparerAndComparison> x, String<TComparerAndComparison> y)
		{
			return !(x == y);
		}
		public static bool operator !=(string x, String<TComparerAndComparison> y)
		{
			return !(x == y);
		}
		public static bool operator !=(String<TComparerAndComparison> x, string y)
		{
			return !(x == y);
		}
		#endregion

		#region IndexOf, LastIndexOf, StartsWith, EndsWith
		public bool EndsWith(string value)
		{
			return Value.EndsWith(value, _comparisonType);
		}

		public int IndexOf(string value)
		{
			return Value.IndexOf(value, _comparisonType);
		}

		public int IndexOf(string value, int startIndex)
		{
			return Value.IndexOf(value, startIndex, _comparisonType);
		}

		public int IndexOf(string value, int startIndex, int count)
		{
			return Value.IndexOf(value, startIndex, count, _comparisonType);
		}

		public int LastIndexOf(string value)
		{
			return Value.LastIndexOf(value, _comparisonType);
		}

		public int LastIndexOf(string value, int startIndex)
		{
			return Value.LastIndexOf(value, startIndex, _comparisonType);
		}

		public int LastIndexOf(string value, int startIndex, int count)
		{
			return Value.LastIndexOf(value, startIndex, count, _comparisonType);
		}

		public bool StartsWith(string value)
		{
			return Value.StartsWith(value, _comparisonType);
		}
		#endregion

	}
}
