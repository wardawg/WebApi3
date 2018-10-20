using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repos.Mapper.Entities
{

    public class MapTarget
    {
        public Type source { get; set; }
        public Type target { get; set; }
        public Type baseType { get; set; }
        public string mapName { get; set; }
        public int inheritOrder { get; set; }
        public Boolean IsCompexMapping { get; set; } = false;
    }

     interface INoInterface
    {

    }

    public static class MapSetter
    {
        private static Func<dynamic,string, dynamic> _funcResolveComplexValue = DefaultComplexType;


        public static object EntityDestination(dynamic entity, Type type)
        {
            return ModelDestination<INoInterface, INoInterface>(entity, type);
        }

        public static object EntityDestination<ITarget, IComplexType>(dynamic entity, Type type)
        {
            return ModelDestination<ITarget, IComplexType>(entity, type);
        }

        public static object ModelDestination(dynamic entity, Type DestType)
        {
            return ModelDestination<INoInterface, INoInterface>(entity, DestType);
        }

        public static object ModelDestination<ITarget, IComplexType>(dynamic entity, Type DestType)
        {

            dynamic dest = Activator.CreateInstance(DestType, true);

            var mapProps = ((IEnumerable<PropertyInfo>)dest
                            .GetType()
                            .GetProperties())
                            .Select(s => s.Name)
                            .Intersect(((IEnumerable<PropertyInfo>)entity
                            .GetType()
                            .GetProperties())
                            .Select(s => s.Name)
                            );

            var Tgtprops = mapProps
                          .ToList()
                          .Select(s => dest.GetType().GetProperty(s))
                          .Where(w => w.CanWrite)
                          .Select(tgt => new
                          {
                              tgt = tgt
                                              ,
                              tgtIsComplexType = typeof(IComplexType)
                                                                .IsAssignableFrom(tgt
                                                                .PropertyType)
                                             ,
                              src = entity
                                                   .GetType()
                                                   .GetProperty(tgt.Name)
                                             ,
                              srcIsComplexType = typeof(IComplexType)
                                                                    .IsAssignableFrom(entity
                                                                    .GetType()
                                                                    .GetProperty(tgt.Name)
                                                                    .PropertyType)
                          }
                                 );


            foreach (var trgprop in Tgtprops)
            {
                var srcValue = default(dynamic);
                var srcValues = default(dynamic);
                var isSourceDirty = false;

                if (!trgprop.srcIsComplexType)
                    srcValue = trgprop.src.GetValue(entity, null);
                else
                {
                    srcValue = trgprop.src.GetValue(entity, null);
                    if (srcValue != null)
                    {
                        isSourceDirty = srcValue.IsDirty;
                        srcValues = srcValue.Attributes;
                    }

                }

                if (srcValue == null)
                    continue;

                if (!trgprop.tgtIsComplexType)
                    if (trgprop.srcIsComplexType)
                        trgprop.tgt.SetValue(dest, _funcResolveComplexValue(srcValue,"Value"), null);
                    else
                        trgprop.tgt.SetValue(dest, srcValue, null);
                else
                {
                    var tgtSource = trgprop.tgt.GetValue(dest, null);

                    if (tgtSource == null)
                        tgtSource = srcValue;
                    else
                    {
                        if (trgprop.srcIsComplexType)
                            tgtSource.Value = _funcResolveComplexValue(srcValue,"Value"); 
                        else
                            tgtSource.Value = srcValue;

                        if (srcValues != null)
                            tgtSource.Attributes = srcValues;

                    }

                    trgprop.tgt.SetValue(dest, tgtSource, null);
                }

            }

            return dest;

        }

        private static dynamic DefaultComplexType(dynamic compleType,string propname)
        {
            return  compleType
                        .GetType()
                        .GetProperty(propname)?
                        .GetValue(compleType);   //compleType[parms];   //  .Value;
        }

        public static bool ContainsColumn(Type source,string columnName)
        {
            return source.GetProperty(columnName, BindingFlags.Public| BindingFlags.IgnoreCase) != null;
        }

        public static bool IscomplexMapping<IComplexType>(MapTarget maps)
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

    }
}
