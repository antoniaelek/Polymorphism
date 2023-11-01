using Newtonsoft.Json;
using Polymorphism;

var pagePreference = new PagePreference()
{
    Id = Guid.NewGuid().ToString("D"),
    Widgets = new List<WidgetPreferenceBase>
    {
        new TablePreference { ResizeColumnsToFit = true },
        new TextGridPreference { ReorderEnabled = false }
    }
};


Console.WriteLine("-----------------------------");
Console.WriteLine("      JSON SERIALIZATION     ");
Console.WriteLine("-----------------------------");

var json = JsonConvert.SerializeObject(pagePreference, Formatting.Indented);
Console.WriteLine(json);

Console.WriteLine("-----------------------------");
Console.WriteLine("     JSON DESERIALIZATION    ");
Console.WriteLine("-----------------------------");

var jsonDeserialized = JsonConvert.DeserializeObject<PagePreference>(json);
foreach (var item in jsonDeserialized?.Widgets ?? Array.Empty<WidgetPreferenceBase>())
{
    Console.WriteLine(item.ToString());
}

Console.WriteLine("-----------------------------");
Console.WriteLine("    BSON (DE)SERIALIZATION   ");
Console.WriteLine("-----------------------------");

var repository = new MongoDbRepository("mongodb://localhost:27017", "polymorphism", "preferences.page");

repository.RegisterClassMaps();

await repository.Insert(pagePreference);

var bsonDeserialized = await repository.Get(pagePreference.Id);

foreach (var item in bsonDeserialized?.Widgets ?? Array.Empty<WidgetPreferenceBase>())
{
    Console.WriteLine(item.ToString());
}