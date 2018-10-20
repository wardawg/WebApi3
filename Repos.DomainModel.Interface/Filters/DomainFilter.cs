using Repos.DomainModel.Interface.Atrributes.DynamicAttributes;
using Repos.DomainModel.Interface.Interfaces.Filter;
using System;
using System.Collections.Generic;

namespace Repos.DomainModel.Interface.Filters
{
    public class DomainFilter<T> : IPropFilter
        where T : AttributeList 
    {
        public string FilterName { set; get; }
        public List<T> Filters { get; set; }
       
        public DomainFilter(string filterName)
        {

            Filters = new List<T>();
            FilterName = filterName;
        }
    }
}
