using Repos.DomainModel.Interface.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Repos.DomainModel.Interface.DomainComplexTypes;

namespace Repos.DomainModel.Interface.Filters
{
    public abstract class EditFilter<T>
        : IEditFilter
        where T : IEntity
    {
        public abstract IQueryable ApplyEditFilter(IQueryable<T> Entity);
        public abstract IEnumerable<string> GetValue(DomainEntityType Entity);
        
    }
}
