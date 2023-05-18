# Changelog

## v1.3 (18-May-2023)

- Added method polyfills for `Stream.ReadAtLeast(...)` and `Stream.ReadAtLeastAsync(...)` method groups.
- Added method polyfills for `Stream.ReadExactly(...)` and `Stream.ReadExactlyAsync(...)` method groups.
- Added type polyfills for `LibraryImportAttribute` and `StringMarshalling`. Note that these polyfills are not enough to enable the source generator for native code marshaling because it only runs on .NET 7 or higher.

## v1.2 (14-Apr-2023)

- Added method polyfills for `Task.WaitAsync(...)` and `Task<T>.WaitAsync(...)` method groups.

## v1.1 (05-Apr-2023)

- Added type polyfills for `ValueTuple<...>`, `HashCode`, `RuntimeInformation`, and `OSPlatform`. These polyfills will be automatically disabled if the corresponding compatibility packages are installed.