using System.IO.Compression;


Console.WriteLine("Hello, World!");

using var destinationStream = new MemoryStream();
ZipFile.CreateFromDirectory(@"C:\temp", destinationStream, CompressionLevel.SmallestSize, false);

Console.WriteLine(destinationStream.Length);
var base64 = Convert.ToBase64String(destinationStream.GetBuffer());
Console.WriteLine(base64);

ZipFile.ExtractToDirectory(destinationStream, @"C:\temp_extracted", true);