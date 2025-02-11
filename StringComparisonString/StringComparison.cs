using StringComparisonEnum = global::System.StringComparison;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace JDanielSmith.System;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class StringComparison
{
	/// <summary>
	/// "Convert" System.StringComparison values into types so they can be used as a generic argument
	/// </summary>
	public interface IStringComparison
	{
		static abstract StringComparisonEnum Comparison { get; }
	}

	/// <summary>
	/// Compare strings using culture-sensitive sort rules and the current culture.
	/// </summary>
	public readonly struct CurrentCulture : IStringComparison
	{
		public static StringComparisonEnum Comparison { get; } = StringComparisonEnum.CurrentCulture;
	}

	/// <summary>
	/// Compare strings using culture-sensitive sort rules, the current culture, and
	/// ignoring the case of the strings being compared.
	/// </summary>
	public readonly struct CurrentCultureIgnoreCase : IStringComparison
	{
		public static StringComparisonEnum Comparison { get; } = StringComparisonEnum.CurrentCultureIgnoreCase;
	}

	/// <summary>
	/// Compare strings using culture-sensitive sort rules and the invariant culture.
	/// </summary>
	public readonly struct InvariantCulture : IStringComparison
	{
		public static StringComparisonEnum Comparison { get; } = StringComparisonEnum.InvariantCulture;
	}

	/// <summary>
	/// Compare strings using culture-sensitive sort rules, the invariant culture, and
	/// ignoring the case of the strings being compared.
	/// </summary>
	public readonly struct InvariantCultureIgnoreCase : IStringComparison
	{
		public static StringComparisonEnum Comparison { get; } = StringComparisonEnum.InvariantCultureIgnoreCase;
	}

	/// <summary>
	/// Compare strings using ordinal sort rules.
	/// </summary>
	public readonly struct Ordinal : IStringComparison
	{
		public static StringComparisonEnum Comparison { get; } = StringComparisonEnum.Ordinal;
	}

	/// <summary>
	/// Compare strings using ordinal sort rules and ignoring the case of the strings
	/// being compared.
	/// </summary>
	public readonly struct OrdinalIgnoreCase : IStringComparison
	{
		public static StringComparisonEnum Comparison { get; } = StringComparisonEnum.OrdinalIgnoreCase;
	}
}