using Vega.Engine.Interfaces.Services;
using Vega.Framework.Data.Entities.Names;

namespace Vega.Engine.Interfaces;

public interface INameService : IVegaService
{

    string RandomName(NameTypeEnum usage, GenderTypeEnum gender = GenderTypeEnum.None);
}
