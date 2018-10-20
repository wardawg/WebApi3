using System.Collections.Generic;
using System.Linq;
using Repos.DomainModel.Interface.DomainComplexTypes;
using Repos.DomainModel.Interface.Filters;
using ReposDomain.Domain;

namespace ReposDomain.Handlers.Handlers
{
    public interface ICategoryTypeEditFilter : IEditFilter{

    }

    public class CategoryTypeEditFilter
           : EditFilter<CategoryType>
           , ICategoryTypeEditFilter
    {
        
        public override IQueryable ApplyEditFilter(IQueryable<CategoryType> Entity)
        {
            return Entity;
        }

        public override IEnumerable<string> GetValue(DomainEntityType Entity)
        {
            throw new System.NotImplementedException();
        }
    }

    public interface ICategoryEditFilter : IEditFilter{
    }

    public interface ISubCategoryEditFilter : IEditFilter
    {
    }

}
