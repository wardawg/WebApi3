using AutoMapper;
using Repos.Mapper.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos.Mapper.Profiles
{
    internal class UpdateEntitiesProfile<IComplexType, ITarget> 
            : MapperProfile<ITarget, IComplexType>
    {

      
        public UpdateEntitiesProfile(IEnumerable<MapTarget> maps)
            :base(maps)
        {
            
        }

        public override string ProfileName => "$update$";
        protected override void Map(IEnumerable<MapTarget> MapTypes)
        {
            foreach (var map in MapTypes)
            {
                var src = map.target;
                var trgt = map.source;

                if (!MapSetter.IscomplexMapping<IComplexType>(map))
                {

                    if (!typeof(ITarget).IsAssignableFrom(map.target))
                    {
                        CreateMap(src, trgt)
                            .ForSourceMember("Id", opt => opt.Ignore());
                    }
                    //.ReverseMap();
                    else
                        if (MapSetter.ContainsColumn(src, "Id"))
                        CreateMap(src, trgt)
                         .ForSourceMember("Id", opt => opt.Ignore());
                    else
                        CreateMap(src, trgt);
                }
                else
                {
                    if (MapSetter.ContainsColumn(src, "Id"))
                        CreateMap(src, trgt)
                        .ForSourceMember("Id", opt => opt.Ignore())
                        .ProjectUsing(s => MapSetter.EntityDestination<ITarget, IComplexType>(s, trgt));
                    else
                        CreateMap(src, trgt)
                        .ProjectUsing(s => MapSetter.EntityDestination<ITarget, IComplexType>(s, trgt));
                }
            }
        }

        protected override IEnumerable<MapTarget> SetMaps()
        {
            return _maps;
        }
    }
}
