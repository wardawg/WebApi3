using Repos.DomainModel.Interface.Interfaces.Filter;
using System.Collections.Generic;

namespace Repos.DomainModel.Interface.Atrributes.DynamicAttributes
{

 
    /// <summary>
    /// 
    /// </summary>
    public interface IFilterKeyPair
        : IPropFilter<IFilterKeyPair>
    {
        string filterValue { set; get; }
        List<string> filterValues { set; get; }
        void SetRef(IFilter refFilter);
         
    }
}