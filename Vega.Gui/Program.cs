using System;
using System.IO;
using System.Threading.Tasks;
using SadConsole;
using SadRogue.Primitives;
using Serilog;
using Vega.Engine;
using Vega.Engine.Interfaces;
using Vega.Framework.Data.Config;
using Vega.Framework.Utils.Random;
using Vega.Gui.Sinks;
using Vega.Gui.Windows;
using Console = SadConsole.Console;

namespace Vega.Gui;

class Program
{
    private static Point _windowSize;

    private static async Task Main(string[] args)
    {
        var options = ConfigService.Instance.Initialize(args);

        InstancesHolder.Manager = new VegaEngineManager(
            new LoggerConfiguration().WriteTo.LogConsoleSink(),
            options
        );
        await InstancesHolder.Manager.Initialize();

        Settings.WindowTitle = $"VegaRL : Days of Darkness - v{InstancesHolder.Manager.GetAssemblyVersion()}";
        Settings.UseDefaultExtendedFont = true;

        Game.Create(
            options.Ui.ScreenWidth * options.Ui.ScaleFactor,
            options.Ui.ScreenHeight * options.Ui.ScaleFactor,
            options.Ui.DefaultUiFont
        );
        _windowSize = new Point(
            options.Ui.ScreenWidth * options.Ui.ScaleFactor,
            options.Ui.ScreenHeight * options.Ui.ScaleFactor
        );

        Game.Instance.UseTitleContainer = true;
        Game.Instance.OnStart = Init;
        Game.Instance.Run();
        Game.Instance.Dispose();
    }

    private static void Init()
    {
        InstancesHolder.Manager.PreloadFonts(Game.Instance);
        // This code uses the default console created for you at start




        InstancesHolder.Manager.LoadServices();
        var worldService = InstancesHolder.Manager.Resolve<IWorldService>();
        var mapConsole = new TestWorldMapWindow(
            _windowSize.X,
            _windowSize.Y,
            worldService.CurrentWorldMap.Width,
            worldService.CurrentWorldMap.Height
        );

        mapConsole.UseKeyboard = true;



        Game.Instance.Screen = mapConsole;
        Game.Instance.DestroyDefaultStartingConsole();
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
