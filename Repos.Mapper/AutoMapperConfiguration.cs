using AutoMapper;
using AutoMapper.Configuration;
using Repos.Mapper.Atrributes;
using Repos.Mapper.Entities;
using Repos.Mapper.Interfaces;
using Repos.Mapper.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Repos.Mapper
{

    internal interface INoMapper{
    }


    /// <summary>
    /// 
    /// </summary>
    public static class AutoMapperConfiguration
    {
       
        private static IMapper _mapper;
        private static Profile _profile = null;
        private static IEnumerable<Profile> _profiles = new List<Profile>();

        private static Func<dynamic, dynamic> _funcResolveComplexValue = DefaultComplexType;

        /// <summary>
        /// 
        /// </summary>
/*
        private class MapTarget
        {
            public Type   source { get; set; }
            public Type   target { get; set; }
            public Type   baseType { get; set; }
            public string mapName { get; set; }
            public int    inheritOrder { get; set; }
        }
        */
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="profiles"></param>
        public static void Init(Profile[] profiles)
        {
            var cfg = new MapperConfigurationExpression();
            foreach (var profile in profiles)
                cfg.AddProfile(profile);

            _mapper = new MapperConfiguration(cfg).CreateMapper();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profile"></param>
        public static void Init(Profile profile)
        {
            _profile = profile;

            Init<INoMapper, INoMapper>(null,false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ISource"></typeparam>
        /// <typeparam name="ITarget"></typeparam>
        /// <typeparam name="IComplexType"></typeparam>
        /// <param name="profile"></param>
        /// <param name="forced"></param>
        public static void Init<ISource,ITarget, IComplexType>(Profile profile,bool forced = false)
        {
            
            _profile = profile;
            Init<ISource, ITarget, IComplexType>(false);
        }


        //public static void Init<ISource, ITarget, IComplexType>(IEnumerable<Profile> profile, bool forced = false)
        //{

        //    _profile = profile;
        //    Init<ISource, ITarget, IComplexType>(false);
        //}
        public static void Init<ISource, ITarget, IComplexType>(Profile profile, Func<dynamic, dynamic> resolveComplexValue, bool forced = false)
        {
            _funcResolveComplexValue = resolveComplexValue;
            Init<ISource, ITarget>(profile, forced);
        }

        //Func<dynamic, dynamic> resolveComplexValue


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ISource"></typeparam>
        /// <typeparam name="ITarget"></typeparam>
        /// <param name="profile"></param>
        /// <param name="forced"></param>
        public static void Init<ISource, ITarget>(Profile profile,bool forced = false)
        {
            Init<ISource, ITarget, INoMapper>(forced);
        }

        public static void Init<ISource, ITarget>(bool forced = false)
        {
            Init<ISource, ITarget, INoMapper>(forced);
        }
            /// <summary>
            /// Initialize mapper
            /// </summary>
        public static void Init<ISource, ITarget, IComplexType>(bool forced = false)
        {

            if (_mapper != null && forced == false)
                return;

            _mapper = null;


              Func<Assembly, IEnumerable<Type>> ValidMapperTypes = (Assembly invalue) =>
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

        Func<Assembly, bool> ValidAssm = (Assembly invalue) =>
            {
                bool ret = true;
                try
                {
                    invalue.GetTypes().Any();
                }
                catch
                {
                    ret = false;
                }

                return ret;
            };


          

            var Types =
                    AppDomain
                    .CurrentDomain
                    .GetAssemblies()
                    .SelectMany(s=> ValidMapperTypes(s))  
                    .Where(i =>!i.GetCustomAttributes(typeof(Atrributes.IgnoreMapAttribute), true)
                                 .Any()
                                &&  !i.IsAbstract 
                                &&  !i.IsInterface 
                                &&  (typeof(ITarget).IsAssignableFrom(i) 
                                    || typeof(IMapperTarget).IsAssignableFrom(i))
                                &&  i.GetInterfaces()
                                        .Any(g => g.IsGenericType
                                              && g.GetType().GetInterfaces().Any()))
                        .Select(map => new MapTarget
                        {
                            target = map
                            ,mapName = map.Name
                            ,baseType = map.BaseType
                            ,source = GetSource<ITarget>(map)
                            ,inheritOrder = GetTypeInheritsNumber(map)
                        }
                    );

            //var Types = maps
            //            .Where(w => w.inheritOrder ==
            //                    maps
            //                       .Where(w2 => w2.source.Name == w.source.Name)
            //                       .Max(m => m.inheritOrder));


            //var haveDups = Types
            //           .Select(s => s)
            //           .GroupBy(g => new { g.target.Name
            //                              ,g.inheritOrder}
            //           )

            //           .Where(w => w.Skip(1).Any())
            //           .SelectMany(sm => sm);


            //if (haveDups != null && haveDups.Any())
            //{
            //    //throw
            //    //    new Exception(
            //    //        string.Format("Found Multiple Mapping for {0}", haveDups.FirstOrDefault().Name)
            //    //        );
            //}

                        
    var cfg = new MapperConfigurationExpression();


    if (_profile != null)
        cfg.AddProfile(_profile);
                         

    if (cfg.Profiles.Count() == 0)
    {
        cfg.AddProfile(new DefaultProfile<IComplexType, ITarget>());
    }

        var custom = cfg
                        .Profiles.Select(s => s.TypeMapConfigs)
                        .SelectMany(sm => sm)
                        .Select(s => s);

            cfg.AddProfile(new UpdateEntitiesProfile<IComplexType,ITarget>(Types));

    foreach (var map in Types
                .Where(w => !custom.Any(ww=> ww.SourceType.Equals(w.source)))
                .OrderByDescending(o=> o.inheritOrder))   // sort by inheritance
    {                                                     // inherited view model objects 
                                                          // must use specific type
                                                          // calls to mapper
       

        if (!IscomplexMapping<IComplexType>(map))
        {
            if (!typeof(ITarget).IsAssignableFrom(map.target))
                cfg.CreateMap(map.source, map.target)
                .ReverseMap();
            else
                cfg.CreateMap(map.source, map.target)
                .ReverseMap();
        }
        else
        {
            cfg.CreateMap(map.source, map.target,MemberList.None)
            .ProjectUsing(src => MapSetter.ModelDestination<ITarget, IComplexType>(src, map.target));

            cfg.CreateMap(map.target, map.source)
            .ProjectUsing(src => MapSetter.EntityDestination<ITarget,IComplexType>(src, map.source));
        }
    }

        
            _mapper = new MapperConfiguration(cfg).CreateMapper();
            _mapper.ConfigurationProvider.CompileMappings();





}

/// <summary>
/// 
/// </summary>
/// <param name="t"></param>
/// <returns></returns>
private static int GetTypeInheritsNumber(Type t)
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
private static Type GetSource<ITarget>(Type t)
{

Type outType = t;
Type out_source = default(Type);
try
{
      out_source = t
                    .GetInterfaces()
                    .Where(s => s.IsGenericType && typeof(ITarget).IsAssignableFrom(s))
                    .DefaultIfEmpty(GetBaseType(t.BaseType))
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
private static Type GetBaseType(Type t)
{
Type outType = t;

if (outType.BaseType != null && outType.BaseType != typeof(Object))
    outType = outType.BaseType;

return outType;
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="IComplexType"></typeparam>
/// <param name="maps"></param>
/// <returns></returns>
private static bool IscomplexMapping<IComplexType>(MapTarget maps)
{

return maps
          .target
          .GetProperties()
          .Any(s => typeof(IComplexType)
                 .IsAssignableFrom(s.PropertyType))

? true :
       maps
         .source
         .GetProperties()
         .Any(s => typeof(IComplexType)
                     .IsAssignableFrom(s.PropertyType));

}


        private static dynamic ResovleComplexType(dynamic complexType)
        {
            return  _funcResolveComplexValue(complexType);
        }

    private static dynamic DefaultComplexType(dynamic compleType)
        {
            return compleType.Value;
        }
/// <summary>
/// Mapper
/// </summary>
public static IMapper Mapper
{
    get
    {
        return _mapper;
    }
}

}


}


