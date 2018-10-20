using Repos.Mapper.Interfaces;
using System;
using System.Linq;

namespace Repos.Mapper.Extentions
{
    public static class MappingExtensions
    {

        private static dynamic SetMap<T>(this T source, string profile = "")
        {
            Type target = source == null ? null : GetMap(source, profile);

            return target == null 
                    ? null : AutoMapperConfiguration
                               .Mapper
                               .Map(source, typeof(T), target);
        }

              
        public static dynamic ToModel<TSource>(this TSource entity)
        {
            return SetMap<TSource>(entity);
        }

        public static Target ToModel<Source, Target>(this Source entity)
        
        {
            return AutoMapperConfiguration.Mapper.Map<Target>(entity);
        }



        public static dynamic ToEntity<TSource>(this TSource model)
        {
            return SetMap<TSource>(model);
        }

        public static dynamic UpdateMap<T>(this T model)
        {
            return SetMap<T>(model,"$update$");
        }
        
      

        public static Target ToEntity<Source, Target>(this Source model)
        {
            return AutoMapperConfiguration
                    .Mapper
                    .Map<Target>(model);
        }

        private static Type GetMap(object model,string profile="")
        {
                       
            Func<string, string, string> ResolveProfileName = (string invalue, string defaultValue) =>
            {

                return String.IsNullOrEmpty(invalue) ? defaultValue : invalue;
            };

           var map = AutoMapperConfiguration
                       .Mapper.ConfigurationProvider
                       .GetAllTypeMaps()
                       .Where(w => w.Profile.Name == ResolveProfileName(profile, w.Profile.Name) &&
                                  (w.SourceType == model.GetType()) 
                                    || w.SourceType == model.GetType().BaseType)
                       .FirstOrDefault();

            if (map == null)
            {
                var sError = String.IsNullOrEmpty(profile)
                    ?  "Could Not Be Resolved For Mapping"
                    : " Could Not Be Resolved For Mapping with Profile " + profile;
            
                throw new NotImplementedException(model.GetType().Name + sError);
            }

            return map.DestinationType;

        }
    }
}
