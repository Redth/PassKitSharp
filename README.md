PassKitSharp
============

PassKitSharp is a C#/.NET Library for Reading and Writing Apple PassKit files.

It's currently not complete, although the parser is working on a basic level.  The parser currently does not check the signature,
but does verify the hashes in the manifest.json for validity.

Sample
------

```csharp
var pk = PassKit.Parse("C:\\pass.pkpass");

Console.WriteLine(pk.Barcode.Message);
```
