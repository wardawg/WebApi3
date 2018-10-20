using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReposServiceConfigurations.ServiceTypes.Attributes
{

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class NoServiceResolveAtrribute : Attribute
    {
    }

  }
