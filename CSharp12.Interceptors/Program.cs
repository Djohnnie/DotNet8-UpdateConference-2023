
using System.Runtime.CompilerServices;

Console.WriteLine("Hello, World!");
Console.WriteLine("-------------");
Console.WriteLine();

Console.WriteLine(Utils.MyRandomMethod(2, 3));



public static class Utils
{
    public static string MyRandomMethod(int a, int b)
    {
        return $"{a} + {b} = {a + b}";
    }
}


namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    sealed class InterceptsLocationAttribute(string filePath, int line, int column) : Attribute
    {
    }
}

namespace Interception
{
    public static class Interceptor
    {
        [InterceptsLocation("C:\\_-_GITHUB_-_\\DotNet8-UpdateConference-2023\\CSharp12.Interceptors\\Program.cs", 8, 25)]
        public static string InterceptedMyRandomMethod(int a, int b)
        {
            return "INTERCEPTED!";
        }
    }
}