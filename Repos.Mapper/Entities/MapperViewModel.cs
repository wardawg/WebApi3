
using Repos.Mapper.Atrributes;
using Repos.Mapper.Interfaces;


namespace Repos.Mapper.Entities
{
    [IgnoreMapAttribute]
    public class MapperViewModel
        :IMapperTarget<MapperSource>
    {
    }
}
