

using System.Security.Cryptography;

Console.WriteLine("Hello, World!");



var data = new byte[1024];
Random.Shared.NextBytes(data);



if (SHA3_256.IsSupported)
{
    byte[] hash = SHA3_256.HashData(data);
    Console.WriteLine(Convert.ToBase64String(hash));
}
else
{
    Console.WriteLine("SHA3 not supported by this platform!");
}


if (SHA3_256.IsSupported)
{
    using ECDsa ec = ECDsa.Create(ECCurve.NamedCurves.nistP256);
    byte[] signature = ec.SignData(data, HashAlgorithmName.SHA3_256);
    Console.WriteLine(Convert.ToBase64String(signature));
}
else
{
    Console.WriteLine("SHA3 not supported by this platform!");
}