namespace Vega.Api.Data.Config;

public class VegaEngineOption
{
    public string RootDirectory { get; set; }

    public VegaUiOption Ui { get; set; } = new();

}
