using Vega.Framework.Attributes;
using Vega.Framework.Data.Entities.Base;

namespace Vega.Framework.Data.Entities.Player;


[EntityData("player_info_def")]
public class PlayerInfoEntity : BaseEntity
{
    public string? ItemGroupId { get; set; }
}
