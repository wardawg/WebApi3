using Repos.DomainModel.Interface.Atrributes.DynamicAttributes;
using Repos.DomainModel.Interface.DomainComplexTypes;
using System;
using System.Collections.Generic;

namespace Repos.DomainModel.Interface.Interfaces.Filter
{

   
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPropFilter<T> : IPropFilter
    {
     
        IEnumerable<T> GetFilterValues(DomainEntityType source);
        IEnumerable<T> GetFilterValues();
        void SetValue(DomainEntityType source);
        void SetValue(DomainEntityType source
                      ,IDomainActionFilter filter
                      ,IEnumerable<IFilterKeyPair> filters);

    }

    
    /// <summary>
    /// 
    /// </summary>
    public interface IPropFilter : IFilter{

    }
}
