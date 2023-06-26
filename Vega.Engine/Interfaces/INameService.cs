using Vega.Api.Data.Entities.Names;
using Vega.Engine.Interfaces.Services;

namespace Vega.Engine.Interfaces;

public interface INameService : IVegaService
{

    string RandomName(NameTypeEnum usage, GenderTypeEnum gender = GenderTypeEnum.None);
}
