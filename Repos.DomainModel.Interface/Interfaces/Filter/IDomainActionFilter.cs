using Repos.DomainModel.Interface.Atrributes.DynamicAttributes;
using Repos.DomainModel.Interface.DomainComplexTypes;
using Repos.DomainModel.Interface.Filters;
using System.Collections.Generic;

namespace Repos.DomainModel.Interface.Interfaces.Filter
{

    public interface IDomainActionFilter : IFilter
    {
      //  void ApplyFilter(FilterParms parms);
        dynamic ReturnValue();
        void SetAttribute(DomainEntityType source);
        void SetAttribute(DomainEntityType source
                         ,IEnumerable<IFilterKeyPair> filters);
    }
}
