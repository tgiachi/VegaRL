﻿using Vega.Engine.Interfaces.Services;

namespace Vega.Engine.Interfaces;

public interface ITextProducerService : IVegaService
{
    string GetTextFromTemplate(string templateName);
    string GetText(string text);
}
