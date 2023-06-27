using System;
using System.IO;
using System.Threading.Tasks;
using SadConsole;
using SadRogue.Primitives;
using Serilog;
using Vega.Engine;
using Vega.Framework.Data.Config;
using Vega.Framework.Utils.Random;
using Vega.Gui.Sinks;
using Console = SadConsole.Console;

namespace Vega.Gui;

class Program
{
    private static async Task Main(string[] args)
    {
        var options = new VegaEngineOption()
        {
            RootDirectory = Path.Join(
                Environment.GetEnvironmentVariable("VEGARL_DATA_DIR") ??
                Path.Join(Directory.GetCurrentDirectory(), "vega")
            )
        };
        InstancesHolder.Manager = new VegaEngineManager(
            new LoggerConfiguration().WriteTo.LogConsoleSink(),
            options
        );
        await InstancesHolder.Manager.Initialize();

        var SCREEN_WIDTH = 80 * 2;
        var SCREEN_HEIGHT = 25 * 2;

        SadConsole.Settings.WindowTitle = "SadConsole Game";
        SadConsole.Settings.UseDefaultExtendedFont = true;

        SadConsole.Game.Create(SCREEN_WIDTH, SCREEN_HEIGHT, options.Ui.DefaultUiFont);

        SadConsole.Game.Instance.OnStart = Init;
        SadConsole.Game.Instance.Run();
        SadConsole.Game.Instance.Dispose();
    }

    private static void Init()
    {
        InstancesHolder.Manager.PreloadFonts(Game.Instance);
        // This code uses the default console created for you at start
        var startingConsole = Game.Instance.StartingConsole;

        startingConsole.FillWithRandomGarbage(SadConsole.Game.Instance.Fonts.Values.RandomElement());
        startingConsole.Fill(new Rectangle(3, 3, 23, 3), Color.Violet, Color.Black, 0, Mirror.None);
        startingConsole.Print(4, 4, "Hello from SadConsole");

        InstancesHolder.Manager.LoadServices();

        // --------------------------------------------------------------
        // This code replaces the default starting console with your own.
        // If you use this code, delete the code above.
        // --------------------------------------------------------------
        /*
        var console = new Console(Game.Instance.ScreenCellsX, SadConsole.Game.Instance.ScreenCellsY);
        console.FillWithRandomGarbage(console.Font);
        console.Fill(new Rectangle(3, 3, 23, 3), Color.Violet, Color.Black, 0, 0);
        console.Print(4, 4, "Hello from SadConsole");

        Game.Instance.Screen = console;

        // This is needed because we replaced the initial screen object with our own.
        Game.Instance.DestroyDefaultStartingConsole();
        */
    }
}
