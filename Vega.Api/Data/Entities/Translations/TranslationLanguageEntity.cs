using Vega.Api.Attributes;
using Vega.Api.Data.Entities.Base;

namespace Vega.Api.Data.Entities.Translations;

[EntityData("translation_language")]
public class TranslationLanguageEntity : BaseEntity
{
    public string Language { get; set; } = null!;

    public Dictionary<string, string> Translations { get; set; } = new();


}
