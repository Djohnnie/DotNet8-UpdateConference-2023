

var possibleValues = Enumerable.Range(0,2).ToArray();

var data = Random.Shared.GetItems(possibleValues, 64);

Console.WriteLine(string.Join("", data));




var randomItems = Enumerable.Range(0, 32).ToArray();

Random.Shared.Shuffle(randomItems);

Console.WriteLine(string.Join(",", randomItems));