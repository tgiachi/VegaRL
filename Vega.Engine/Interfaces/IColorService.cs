﻿using SadRogue.Primitives;
using Vega.Engine.Interfaces.Services;

namespace Vega.Engine.Interfaces;

public interface IColorService : IVegaService, IVegaReloadableService
{

    Color GetColorByName(string name);

}
