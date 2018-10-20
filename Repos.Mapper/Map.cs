using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos.Mapper
{
   public class Mapper
    {
        public static TDestination Map<TSource, TDestination>(TSource source)
        {
            return default(TDestination) ;
        }

        public static dynamic Map(object TSource, object TDestination)
        {
            return default(object);
        }
    }
}
