using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos.DomainModel.Interface.Interfaces.DomainList
{
   public interface IListFactory
    {
        T Using<T>() where T : class;

        //IDomainList GetList(    );
    }
}
