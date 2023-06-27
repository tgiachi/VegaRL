using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Vega.Engine.Interfaces;
using Vega.Engine.Services.Base;
using Vega.Framework.Attributes;
using Vega.Framework.Data.Entities.Text;
using Vega.Framework.Utils;

namespace Vega.Engine.Services;

[VegaService(10)]
public partial class TextProducerService : BaseDataLoaderVegaService<TextProducerService>, ITextProducerService
{
    [GeneratedRegex("\\$\\{(.*?)\\}", RegexOptions.Compiled)]
    private static partial Regex TextProducerTokenRegex();

    private Dictionary<string, (object, MethodInfo)> _textProducers = new();

    private readonly Dictionary<string, string> _texts = new();
    private readonly IServiceProvider _serviceProvider;
    private readonly ITranslationService _translationService;

    /// <summary>
    ///   Regex to extract tokens from text.
    ///  Tokens are in the format of ${token}
    /// </summary>
    private readonly Regex _tokenExtractExpression = TextProducerTokenRegex();

    public TextProducerService(
        ILogger<TextProducerService> logger, IDataService dataService, IMessageBusService messageBusService,
        IServiceProvider serviceProvider, ITranslationService translationService
    ) : base(logger, dataService, messageBusService)
    {
        _serviceProvider = serviceProvider;
        _translationService = translationService;
    }

    public override Task<bool> LoadAsync()
    {
        LoadTexts();
        LoadTextProducers();
        return Task.FromResult(true);
    }

    private Task LoadTextProducers()
    {
        foreach (var type in AssemblyUtils.GetAttribute<TextProducerAttribute>())
        {
            var attribute = type.GetCustomAttribute<TextProducerAttribute>();
            var service = _serviceProvider.GetService(type);
            if (service == null)
            {
                Logger.LogError("Failed to load text producer {TypeName}", type.Name);
                throw new Exception($"Failed to load text producer {type.Name}");
            }

            foreach (var method in type.GetMethods())
            {
                if (method.GetCustomAttribute<TextProducerValueAttribute>() != null)
                {
                    var methodAttribute = method.GetCustomAttribute<TextProducerValueAttribute>();
                    var key = $"{attribute.Suffix}.{methodAttribute}";
                    _textProducers.Add(key, (service, method));
                }
            }
        }

        return Task.CompletedTask;
    }

    private Task LoadTexts()
    {
        foreach (var textEntity in LoadData<TextEntity>())
        {
            _texts.Add(textEntity.Id, string.Join(Environment.NewLine, textEntity.Lines));
        }

        return Task.CompletedTask;
    }

    public string GetTextFromTemplate(string templateName)
    {
        if (_texts.TryGetValue(templateName, out var text))
        {
            return GetText(text);
        }

        throw new Exception($" Text with id '{templateName}' not found");
    }

    public string GetText(string text)
    {
        var fullText = text;
        foreach (Match m in _tokenExtractExpression.Matches(text))
        {
            var textToReplace = m.Value.Replace("${", "").Replace("}", "");
            if (!_textProducers.ContainsKey(textToReplace))
            {
                Logger.LogWarning("Text with id '{TextId}' not found", textToReplace);
                continue;
            }

            var result = _textProducers[textToReplace].Item2.Invoke(_textProducers[textToReplace].Item1, null).ToString();
            fullText = fullText.Replace(m.Value, result);
        }

        return _translationService.Translate(fullText);
    }
}
