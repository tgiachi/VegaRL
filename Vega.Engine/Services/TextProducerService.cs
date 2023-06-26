using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Vega.Api.Attributes;
using Vega.Engine.Interfaces;
using Vega.Engine.Services.Base;

namespace Vega.Engine.Services;

[VegaService(10)]
public partial class TextProducerService : BaseVegaService<TextProducerService>, ITextProducerService
{
    [GeneratedRegex("\\$\\{(.*?)\\}", RegexOptions.Compiled)]
    private static partial Regex TextTokenRegex();

    /// <summary>
    ///   Regex to extract tokens from text.
    ///  Tokens are in the format of ${token}
    /// </summary>
    private readonly Regex _tokenExtractExpression = TextTokenRegex();

    public TextProducerService(ILogger<TextProducerService> logger, IMessageBusService messageBusService) : base(logger, messageBusService)
    {
    }
}
