using Vega.Api.Attributes;
using Vega.Api.Data.Entities.Base;

namespace Vega.Api.Data.Entities.Keybindings;

[EntityData("keybinding_def")]
public class KeybindingEntity : BaseEntity, IHasCategory
{
    public string Category { get; set; }
    public string? SubCategory { get; set; }

    public string? Key { get; set; }
    public bool IsAlt { get; set; }
    public bool IsCtrl { get; set; }
    public string? Command { get; set; }
}
