# StringComparisonString

Provide a wrapper around `System.String` which automatically uses `TStringComparison` instead of having
to explicitly specify a `System.StringComparison` value.

This is especially useful when using `string`s as keys in collections, where the key is something like a Windows file-system pathname;
it can be easy to forget to pass an `IEqualityComparer<>` in the constructor.
