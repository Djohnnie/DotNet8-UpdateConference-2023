

using System.Collections;
using System.Runtime.CompilerServices;

Console.WriteLine("Hello, World!");




int[] a1 = { 2, 3, 4 };

int[] a2 = [6, 7, 8];
int[] a = [1, .. a1, 5, .. a2, 9];

for (int i = 0; i < a.Length; i++)
{
    Console.WriteLine($"a[{i}] = {a[i]}");
}


int[][] identity = [[1, 0, 0], [0, 1, 0], [0, 0, 1]];

Span<char> chars = ['a', 'b', 'c', 'd', 'e'];
char[] chars2 = [ 'a', 'b', 'c', 'd', 'e' ];
char[] chars3 = { 'a', 'b', 'c', 'd', 'e' };

CharBuffer buffer = ['H', 'e', 'l', 'l', 'o'];





[CollectionBuilder(typeof(CharBufferBuilder), "Create")]
public class CharBuffer : IEnumerable<char>
{
    private readonly char[] _buffer = new char[80];

    public CharBuffer(ReadOnlySpan<char> buffer)
    {
        int number = (_buffer.Length < buffer.Length) ? _buffer.Length : buffer.Length;
        for (int i = 0; i < number; i++)
        {
            _buffer[i] = buffer[i];
        }
    }

    public IEnumerator<char> GetEnumerator() => _buffer.AsEnumerable<char>().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _buffer.GetEnumerator();
}

internal static class CharBufferBuilder
{
    internal static CharBuffer Create(ReadOnlySpan<char> values) => new CharBuffer(values);
}