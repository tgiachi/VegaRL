using System;
using System.IO;
using System.Threading.Tasks;
using SadConsole;
using SadRogue.Primitives;
using Serilog;
using Vega.Api.Data.Config;
using Vega.Engine;
using Console = SadConsole.Console;

namespace Vega.Gui;

class Program
{
    private static async Task Main(string[] args)
    {
        InstancesHolder.Manager = new VegaEngineManager(new LoggerConfiguration(), new VegaEngineOption()
        {
            RootDirectory = Path.Join(@"C:\Users\squid\OneDrive\vegarl", "vega")
        });
        await InstancesHolder.Manager.Initialize();

        var SCREEN_WIDTH = 80 * 2;
        var SCREEN_HEIGHT = 25 * 2;

        SadConsole.Settings.WindowTitle = "SadConsole Game";
        SadConsole.Settings.UseDefaultExtendedFont = true;

        SadConsole.Game.Create(SCREEN_WIDTH, SCREEN_HEIGHT);
        SadConsole.Game.Instance.OnStart = Init;
        SadConsole.Game.Instance.Run();
        SadConsole.Game.Instance.Dispose();
    }

    private static void Init()
    {
        // This code uses the default console created for you at start
        var startingConsole = Game.Instance.StartingConsole;

        startingConsole.FillWithRandomGarbage(SadConsole.Game.Instance.StartingConsole.Font);
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
