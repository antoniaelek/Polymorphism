using JsonSubTypes;
using Newtonsoft.Json;

namespace Polymorphism;

public enum WidgetPreferenceType
{
    Table,
    TextGrid
}

public class PagePreference
{
    public string Id { get; set; }
    public IEnumerable<WidgetPreferenceBase> Widgets { get; set; }
}

[JsonConverter(typeof(JsonSubtypes), "Type")]
[JsonSubtypes.KnownSubType(typeof(TablePreference), WidgetPreferenceType.Table)]
[JsonSubtypes.KnownSubType(typeof(TextGridPreference), WidgetPreferenceType.TextGrid)]
public abstract class WidgetPreferenceBase
{
    public abstract WidgetPreferenceType Type { get; }
}

public class TablePreference : WidgetPreferenceBase
{
    public override WidgetPreferenceType Type => WidgetPreferenceType.Table;
    public bool ResizeColumnsToFit { get; set; }

    public override string ToString()
    {
        return $"Type: {Type}\n  ResizeColumnsToFit: {ResizeColumnsToFit}\n";
    }
}

public class TextGridPreference : WidgetPreferenceBase
{
    public override WidgetPreferenceType Type => WidgetPreferenceType.TextGrid;
    public bool ReorderEnabled { get; set; }

    public override string ToString()
    {
        return $"Type: {Type}\n  ReorderEnabled: {ReorderEnabled}\n";
    }
}