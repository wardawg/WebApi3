using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyResolver.FuncInterfaces
{
    /// <summary>
    /// Used to couple 
    /// Util Name Resolve Function
    /// </summary>
    public class FuncDependencyResolver
    {
        private Func<Type, string, string, IConfigOptions, string> getResolveValue;
        public FuncDependencyResolver(Func<Type, string, string, IConfigOptions, string> func)
        {
            getResolveValue = func;
        }

        public string ResolveTypeName(Type t
                                            , string name = ""
                                            , string postFix = ""
                                            , IConfigOptions opts = null)
        {
            return getResolveValue(t, name, postFix, opts);
        }
    }
}
