using Repos.DomainModel.Interface.Atrributes.DynamicAttributes;
using Repos.DomainModel.Interface.DomainComplexTypes;
using Repos.DomainModel.Interface.Interfaces;
using Repos.DomainModel.Interface.Interfaces.Filter;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Repos.DomainModel.Interface.Filters
{
    public interface IEditFilter
                : IFilter{

        IEnumerable<string> GetValue(DomainEntityType Entity);
            
    }

    public interface IEditFilter<T>
                : IFilter
    {

        IQueryable ApplyFilter<TFilter>(T Entity)
            where TFilter : IEditFilter;

        
    }




}
