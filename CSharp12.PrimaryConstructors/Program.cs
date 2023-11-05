Console.WriteLine("Placeholder");



public class Student(string name, string firstName, byte age)
{
    public Student(string name, string firstName) : this(name, firstName, 18)
    {

    }

    public string Name => name;

    public string FirstName => firstName;

    public byte Age => age;
}