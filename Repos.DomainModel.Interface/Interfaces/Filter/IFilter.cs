using Repos.DomainModel.Interface.DomainComplexTypes;
using System;
using System.Collections.Generic;

namespace Repos.DomainModel.Interface.Interfaces.Filter
{

    /// <summary>
    /// 
    /// </summary>
    public interface IFilterEnum
    {
        Enum FilterEnum { set; get; }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFilter<T> : IFilter{
              
    }

    
    /// <summary>
    /// 
    /// </summary>
    public interface IFilter{
    }
}
