using System;
using SadConsole;
using SadConsole.Input;
using SadRogue.Integration.FieldOfView.Memory;
using SadRogue.Primitives;
using Vega.Engine.Interfaces;
using Vega.Framework.Map.WorldMap.GameObjects;
using Console = SadConsole.Console;

namespace Vega.Gui.Windows;

public class TestWorldMapWindow : Console
{
    private readonly IWorldService _worldService;

    public TestWorldMapWindow(int width, int height, int bufferWidth, int bufferHeight) : base(
        width,
        height,
        bufferWidth,
        bufferHeight
    )
    {
        IsFocused = true;
        UseKeyboard = true;
        UseMouse = true;
        _worldService = InstancesHolder.Manager.Resolve<IWorldService>();
        Draw();
    }

    public override bool ProcessMouse(MouseScreenObjectState state)
    {
        if (state.Mouse.RightClicked)
        {
            ViewPosition = state.CellPosition;
            Draw();
            return true;
        }
        return false;
    }

    public override bool ProcessKeyboard(Keyboard keyboard)
    {
        if (keyboard.IsKeyPressed(Keys.W))
        {
            ViewPosition += new Point(0, -1);
            Draw();
            return true;
        }
        else if (keyboard.IsKeyPressed(Keys.S))
        {
            ViewPosition += new Point(0, 1);
            Draw();
            return true;
        }
        else if (keyboard.IsKeyPressed(Keys.A))
        {
            ViewPosition += new Point(-1, 0);
            Draw();
            return true;
        }
        else if (keyboard.IsKeyPressed(Keys.D))
        {
            ViewPosition += new Point(1, 0);
            Draw();
        }

        return base.ProcessKeyboard(keyboard);
    }

    public void Draw()
    {
        for (int x = 0; x < ViewWidth + ViewPosition.X; x++)
        {
            for (int y = 0; y < ViewHeight + ViewPosition.Y; y++)
            {
                var tile = _worldService.CurrentWorldMap.Terrain[x, y] as MemoryAwareRogueLikeCell;
                if (tile != null)
                {
                    this.SetGlyph(x, y, tile!.Appearance);
                }
                
            }
        }
    }
}
