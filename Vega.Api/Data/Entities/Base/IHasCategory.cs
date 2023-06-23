namespace Vega.Api.Data.Entities.Base;

public interface IHasCategory
{
    string Category { get; set; }

    string? SubCategory { get; set; }

}
