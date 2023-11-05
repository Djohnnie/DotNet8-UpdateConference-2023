using System.Runtime.CompilerServices;


Console.WriteLine("Hello, World!");


var a = new FixedArray();



for (int i = 0; i < 25; i++)
{
    a[i] = i + 1;
}

var index = 0;
foreach (var e in a)
{
    Console.WriteLine($"a[{index}] = {e}");
    index++;
}



[InlineArray(25)]
public struct FixedArray
{
    private int _element0;
}