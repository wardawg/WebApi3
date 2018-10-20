using AutoMapper;
using Repos.Mapper.Entities;
using Repos.Mapper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Repos.Mapper.Profiles
{
    public abstract class MapperProfile<IComplexType, ITarget> : Profile
    {
        Action ResolveTypes;

        protected abstract IEnumerable<MapTarget> SetMaps();
        protected IEnumerable<MapTarget> _maps = new List<MapTarget>();

        protected MapperProfile(IEnumerable<MapTarget> maps)
        {
            //_maps = maps;
            Map(maps);
        }

        protected MapperProfile()
        {
            Map(SetMaps());
        }

        protected MapperProfile(Action resolver)
        {
            ResolveTypes = resolver;
        }

        
        public override string ProfileName => "MapperProfile";

        protected virtual void Map(IEnumerable<MapTarget> MapTypes)
        {

            ShouldMapProperty = pi => pi.PropertyType != typeof(IList<>);


            foreach (var map in MapTypes)
            {
                if (!MapSetter.IscomplexMapping<IComplexType>(map) && !map.IsCompexMapping )
                {
                    if (!typeof(ITarget).IsAssignableFrom(map.target) )
                        CreateMap(map.source, map.target)
                        .ReverseMap();
                    else
                        CreateMap(map.source, map.target)
                       .ReverseMap();
                }
                else
                {
                    if(map.IsCompexMapping)
                    {
                       CreateMap(map.source, map.target, MemberList.None)
                        .ProjectUsing(src => MapSetter.ModelDestination(src, map.target));

                       CreateMap(map.target, map.source)
                       .ProjectUsing(src => MapSetter.EntityDestination(src, map.source));
                    }
                    else
                    {
                     CreateMap(map.source, map.target, MemberList.None)
                        .ProjectUsing(src => MapSetter.ModelDestination<ITarget, IComplexType>(src, map.target));

                     CreateMap(map.target, map.source)
                        .ProjectUsing(src => MapSetter.EntityDestination<ITarget, IComplexType>(src, map.source));
                    }
                }
            }
        }

        protected int GetTypeInheritsNumber(Type t)
        {
            Type tempType = t;
            int typeOrder = 0;

            do
                if (tempType.BaseType != null && tempType.BaseType != typeof(Object))
                {
                    typeOrder++;
                    tempType = tempType.BaseType;
                }
                else
                    break;

            while (true);

            return typeOrder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ITarget"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        protected Type GetSource<Ttarget>(Type t)
        {

            Type outType = t;
            Type out_source = default(Type);
            try
            {
                    out_source = t
                                .GetInterfaces()
                                .Where(s => s.IsGenericType && 
                                        (typeof(Ttarget).IsAssignableFrom(s) || typeof(IMapperTarget).IsAssignableFrom(s)))
                                .DefaultIfEmpty(GetBaseType(t))
                                .FirstOrDefault()
                                ?.GenericTypeArguments
                                .First();
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return out_source;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        protected Type GetBaseType(Type t)
        {

            Type outType = t;

            if (outType.BaseType != null && outType.BaseType != typeof(Object))
                outType = outType.BaseType;

            return outType;


            //Type outType = t;
            //do
            //    if (outType.BaseType != null && outType.BaseType != typeof(Object))
            //    {
            //        outType = outType.BaseType;

            //        if (outType.GetGenericArguments()
            //            .Any(w => typeof(IMapperSource).IsAssignableFrom(w)))
            //            break;

            //    }
            //    else
            //        break;
            //while (true);


            //return outType;
        }

        protected Func<Assembly, IEnumerable<Type>> ValidMapperTypes = (Assembly invalue) =>
        {
            IEnumerable<Type> types = new List<Type>();
            try
            {
                types = invalue.GetTypes();
            }
            catch
            {

            }

            return types;
        };
    }
}


