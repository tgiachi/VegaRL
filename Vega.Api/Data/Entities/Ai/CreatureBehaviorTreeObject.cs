using Vega.Api.Attributes;
using Vega.Api.Data.Entities.Base;

namespace Vega.Api.Data.Entities.Ai;

[EntityData("behavior_tree")]
public class CreatureBehaviorTreeObject : BaseEntity
{
    public List<CreatureBehaviorNode> Nodes { get; set; }

    public override string ToString() => $"{Id} - {Name} - Nodes: {Nodes.Count}";
}
