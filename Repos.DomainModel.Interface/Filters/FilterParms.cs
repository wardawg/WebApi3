using Repos.DomainModel.Interface.Interfaces.Filter;
using System;
using System.Collections.Generic;

namespace Repos.DomainModel.Interface.Filters
{
    public struct FilterParms : IFilter
    {
        public dynamic FilterSource { get; set; }
        public Dictionary<string,object> ParmInputs { get; set; }
        public IFilterFactory Factory { set; get; }
        public Enum FilterEnum { get; set; }
        public string CustomFilterName { get; set; }
               
    }

    


}
