using Vega.Api.Attributes;
using Vega.Api.Data.Entities.Base;

namespace Vega.Api.Data.Entities.Player;


[EntityData("player_info_def")]
public class PlayerInfoEntity : BaseEntity
{
    public string? ItemGroupId { get; set; }
}
