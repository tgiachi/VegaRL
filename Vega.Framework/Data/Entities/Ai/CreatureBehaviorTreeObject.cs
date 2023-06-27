using Vega.Framework.Attributes;
using Vega.Framework.Data.Entities.Base;

namespace Vega.Framework.Data.Entities.Ai;

[EntityData("behavior_tree")]
public class CreatureBehaviorTreeObject : BaseEntity
{
    public List<CreatureBehaviorNode> Nodes { get; set; }

    public override string ToString() => $"{Id} - {Name} - Nodes: {Nodes.Count}";
}
