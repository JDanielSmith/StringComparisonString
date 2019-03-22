# CaseInsensitiveString
A wrapper around `System.String` to provide case-insensitivity

This is especially useful when using strings as keys in collections, where the key is something like a Windows file-system pathname;
it can be easy to forget to pass an `IEqualityComparer<>` in the constructor.
