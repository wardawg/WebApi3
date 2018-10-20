using Repos.DomainModel.Interface.Atrributes.DynamicAttributes;
using Repos.DomainModel.Interface.Filters;
using System;


namespace Repos.DomainModel.Interface.Interfaces.Filter
{
    public interface IFilterFactory
    {
        IFilter GetFilter(FilterParms AvailableFilters);
        IFilterConstants FilterConstants {get;}
    }
}

