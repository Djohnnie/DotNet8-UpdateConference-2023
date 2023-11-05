using System.Text.Json;
using System.Text.Json.Serialization;

var json = """
    {
        "Name":"John",
        "FirstName":"Doe",
        "Age":44
    }
    """;

var person1 = JsonSerializer.Deserialize<Person>(json);

try
{
    var person2 = JsonSerializer.Deserialize<Person>(json, new JsonSerializerOptions
    {
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow
    });
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}


// {"name":"John","first_name":"Doe"}
var serialized1 = JsonSerializer.Serialize(person1, new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
});

Console.WriteLine(serialized1);


// {"NAME":"John","FIRST-NAME":"Doe"}
var serialized2 = JsonSerializer.Serialize(person1, new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.KebabCaseUpper
});

Console.WriteLine(serialized2);



record Person(string Name, string FirstName);