# Changelog

## v1.5 (05-Jun-2023)

- Added polyfills for `Stream.CopyTo(...)`.
- Added polyfills for `AggregateException`.

## v1.4 (25-May-2023)

- Added polyfills for `IEnumerable<T>.Min(...)`, `IEnumerable<T>.MinBy(...)`, `IEnumerable<T>.Max(...)`, and `IEnumerable<T>.MaxBy(...)`.
- Added polyfills for `IEnumerable<T>.Order(...)` and `IEnumerable<T>.OrderDescending(...)`.
- Added polyfills for `IEnumerable<T>.DistinctBy(...)`, `IEnumerable<T>.ExceptBy(...)`, `IEnumerable<T>.IntersectBy(...)`, and `IEnumerable<T>.UnionBy(...)`.
- Added polyfills for `IEnumerable<T>.Take(...)`, `IEnumerable<T>.TakeLast(...)`, and `IEnumerable<T>.SkipLast(...)`.
- Added polyfills for `IEnumerable<T>.ElementAt(...)` and `IEnumerable<T>.ElementAtOrDefault(...)`.
- Added polyfills for `IEnumerable<T>.FirstOrDefault(...)`, `IEnumerable<T>.LastOrDefault(...)`, and `IEnumerable<T>.SingleOrDefault(...)`.
- Added polyfills for `IEnumerable<T>.Chunk(...)`.
- Added polyfills for `StringSyntaxAttribute`.
- Fixed a typo in the naming of `DisallowNullAttribute`.

## v1.3 (18-May-2023)

- Added polyfills for `Stream.ReadAtLeast(...)` and `Stream.ReadAtLeastAsync(...)`.
- Added polyfills for `Stream.ReadExactly(...)` and `Stream.ReadExactlyAsync(...)`.
- Added polyfills for `LibraryImportAttribute` and `StringMarshalling`. Note that these polyfills are not enough to enable the source generator for native code marshaling because it only runs on .NET 7 or higher.

## v1.2 (14-Apr-2023)

- Added polyfills for `Task.WaitAsync(...)` and `Task<T>.WaitAsync(...)`.

## v1.1 (05-Apr-2023)

- Added polyfills for `ValueTuple<...>`, `HashCode`, `RuntimeInformation`, and `OSPlatform`. These polyfills will be automatically disabled if the corresponding compatibility packages are installed.