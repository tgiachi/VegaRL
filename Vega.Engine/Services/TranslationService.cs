using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Vega.Engine.Interfaces;
using Vega.Engine.Services.Base;
using Vega.Framework.Attributes;
using Vega.Framework.Data.Config;
using Vega.Framework.Data.Entities.Translations;

namespace Vega.Engine.Services;

[VegaService(4)]
public partial class TranslationService : BaseDataLoaderVegaService<TranslationService>, ITranslationService
{
    [GeneratedRegex("\\$\\[(.*?)\\]", RegexOptions.Compiled)]
    private static partial Regex TextTokenRegex();

    /// <summary>
    ///   Regex to extract tokens from text.
    ///  Tokens are in the format of $[token]
    /// </summary>
    private readonly Regex _tokenExtractExpression = TextTokenRegex();


    public string SelectedLanguage { get; set; }

    private readonly VegaEngineOption _options;
    private readonly Dictionary<string, TranslationLanguageEntity> _availableLanguages = new();
    private Dictionary<string, string> _translations = new();


    public TranslationService(ILogger<TranslationService> logger, IDataService dataService, VegaEngineOption options, IMessageBusService messageBusService) : base(
        logger,
        dataService,
        messageBusService
    )
    {
        _options = options;
    }

    public override Task<bool> LoadAsync()
    {
        foreach (var language in LoadData<TranslationLanguageEntity>())
        {
            if (_availableLanguages.ContainsKey(language.Language.ToLower()))
            {
                foreach (var lang in language.Translations)
                {
                    _availableLanguages[language.Language.ToLower()].Translations.Add(lang.Key, lang.Value);
                }
            }
            else
            {
                _availableLanguages.Add(language.Language.ToLower(), language);
            }
        }

        if (_availableLanguages.TryGetValue(_options.DefaultLanguage.ToLower(), out TranslationLanguageEntity value))
        {
            SelectedLanguage = _options.DefaultLanguage.ToLower();
            _translations = value.Translations;
            return Task.FromResult(true);
        }

        throw new Exception($"Language {_options.DefaultLanguage} not found!");
    }

    public string Translate(string text, params object[]? args)
    {
        var fullText = text;
        foreach (Match m in _tokenExtractExpression.Matches(text))
        {
            var textToReplace = m.Value.Replace("$[", "").Replace("]", "");
            if (!_translations.ContainsKey(textToReplace))
            {
                Logger.LogWarning("Text with id '{TextId}' not found", textToReplace);
                continue;
            }

            var result = string.Format(textToReplace, args);
            fullText = fullText.Replace(m.Value, result);
        }

        return fullText;
    }
}
