using Vega.Framework.Attributes;
using Vega.Framework.Data.Entities.Base;

namespace Vega.Framework.Data.Entities.Translations;

[EntityData("translation_language")]
public class TranslationLanguageEntity : BaseEntity
{
    public string Language { get; set; } = null!;

    public Dictionary<string, string> Translations { get; set; } = new();


}
