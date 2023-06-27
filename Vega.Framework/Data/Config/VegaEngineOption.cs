﻿namespace Vega.Framework.Data.Config;

public class VegaEngineOption
{
    public string RootDirectory { get; set; }

    public VegaUiOption Ui { get; set; } = new();

    public string DefaultLanguage { get; set; } = "en";

}