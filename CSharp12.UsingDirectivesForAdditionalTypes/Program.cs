using NullableGuid = System.Guid?;
using Ints = int[];
using ComplexNumber = (decimal Real, decimal Imaginary);



NullableGuid guid = Guid.Empty;
guid = null;


var doStuff = (Ints i) => Console.WriteLine(i.Length);
doStuff(new[] { 1, 2, 3 });


var complexNumber = new ComplexNumber
{
    Real = 1.0m,
    Imaginary = 2.0m
};

Console.WriteLine(
    $"{complexNumber.Real} + {complexNumber.Imaginary}i");