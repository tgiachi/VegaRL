namespace Vega.Engine.Interfaces;

public interface ITranslationService
{
    string Translate(string text, params object []? args);

    string SelectedLanguage { get; set; }
}
