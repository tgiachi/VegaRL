namespace Vega.Framework.Data.Entities.Base;

public interface IHasCategory
{
    string Category { get; set; }

    string? SubCategory { get; set; }

}
