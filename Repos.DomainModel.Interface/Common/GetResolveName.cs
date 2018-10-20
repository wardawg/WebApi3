using System;
using System.Linq;

namespace Repos.DomainModel.Interface.Common
{
    public static class CommonUtil
    {
       
        public static string GetResolveName(Type t,string Name = "",string postFix="")
        {
            string TypeName = t.Name;
            if (!string.IsNullOrEmpty(Name))
                TypeName = Name;

            string name;
            var temp = t.FullName.Split('.');
            if (!string.IsNullOrEmpty(postFix))
                name = temp.First();
            else
                name = string.Join(".", temp.Take(temp.Length-1));

            var sreturn = name + "." + (string.IsNullOrEmpty(postFix) ? "" :  postFix + ".") + TypeName;

            return sreturn;

        }
        
        private static string xGetResolveName(Type t, string Name = "", string postFix = "")
        {

            var temp = t.FullName.Split('.');
            var name = string.Join(".", temp.Take(temp.Length - 1));
            var resolveName = name + "." + Name;
                        

            if (! string.IsNullOrEmpty(Name) && Name.Split('.').Count()>1)
                resolveName = Name;
            //else
            //    if (string.IsNullOrEmpty(Name))
            //    {
            //        Name = t.Name;
            //        temp = t.FullName.Split('.');
            //        name = string.Join(".", temp.Take(temp.Length - 1));
            //        resolveName = name  + "." + Name;
            //    }
               
            return resolveName;
        }
    }

}
