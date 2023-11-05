using System.Collections.Frozen;


var configData = new Dictionary<string, bool>()
{
    {"setting1", true },
    {"setting2", false}
};

var frozenConfigData = configData.ToFrozenDictionary();



var listData = Enumerable.Range(0, 1000);

var frozenListData = listData.ToFrozenSet();