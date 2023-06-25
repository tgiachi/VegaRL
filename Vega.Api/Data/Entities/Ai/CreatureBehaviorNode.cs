namespace Vega.Api.Data.Entities.Ai;

public class CreatureBehaviorNode
{
    public string Id { get; set; }
    public string ActionName { get; set; }
    public Dictionary<string, object> Parameters { get; set; } = new();
    public List<CreatureBehaviorNode> Nodes { get; set; } = new();

    public override string ToString() => $"{Id} - {ActionName} - Nodes: {Nodes.Count}";
}
