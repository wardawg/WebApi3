using DependencyResolver;
using ProjectDependResolver;
using ReposServiceConfigurations.ServiceTypes.Enums;
using System;
using System.Linq;

namespace ReposServiceConfigurations.Common
{
    public static class CommonUtil
    {

        public enum ResolveDepName
        {
            YES
           , NO
        }

        public static bool ResolveDepencyName { get; private set; } = false;

        //public static void SetResolveNameFlag(ResolveDepName resolveName)
        //{
        //    if (resolveName == ResolveDepName.YES)
        //        ResolveDepencyName = true;
        //}

        
      //  public static string ResolveRuleName(string prefix, Type t)
     //   {

      //  }

        public static string GetResolveName(Type t
                                            ,string Name = ""
                                            ,EnumServiceTypes postFix= null
                                            , IConfigOptions Opts = default(IConfigOptions))
        {

           
            string TypeName = t.Name;
            
            if (!string.IsNullOrEmpty(Name))
                TypeName = Name;

            else
                if (t.IsInterface)
                 TypeName = TypeName.Substring(1);

            //if (!ResolveDepencyName && !(Opts != null 
            //                            && Opts
            //                                .Contains(enumConfigOpts.ForceNameResolve))
            //   )
            //        return TypeName;
                
            string name;
            var temp = t.FullName.Split('.');
            if (postFix != null)
                name = temp.First();
            else
                name = string.Join(".", temp.Take(temp.Length-1));

            string sreturn = string.Empty;

            var Typeprefix = String
                            .Join(".",
                               t.FullName.Split('.')
                               .DefaultIfEmpty(".")
                               .Take(2));

            //   return prefix + '.' + TypeName; 
                      
            //need only dll base assm name & service type name
            var compare = String.Join("." , name.Split('.').Take(2));

            if (TypeName.Split('.').Length>2)
                sreturn = TypeName;
            else
                if (compare.Contains(Typeprefix))
                    sreturn = Typeprefix + '.' + TypeName;
                else
                    sreturn = name + "." + (postFix == null ? "" :  postFix + ".") + TypeName;

            return sreturn;

        }
                
    }

}
