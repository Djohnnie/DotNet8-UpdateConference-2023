using System.ComponentModel.DataAnnotations;

Console.WriteLine("Placeholder");


class ViewModel
{
    [Range(0d, 1d, MinimumIsExclusive = true, MaximumIsExclusive = true)]
    public double Sample { get; set; }


    [Length(10, 20)]
    public ICollection<int> Values { get; set; }


    [AllowedValues("apple", "banana", "mango")]
    public string Fruit { get; set; }


    [DeniedValues("pineapple", "anchovy", "broccoli")]
    public string PizzaTopping { get; set; }


    [Base64String]
    public string Data { get; set; }
}