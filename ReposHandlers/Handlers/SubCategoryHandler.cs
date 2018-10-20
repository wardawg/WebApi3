using ReposData.Repository;
using ReposDomain.Domain;
using ReposServiceConfigurations.ServiceTypes.Base;
using ReposServiceConfigures.ServiceTypes.Handlers;
using System.Linq;

namespace ReposDomain.Handlers.Handlers
{
    public class SubCategoryHandler
        : ServiceGenericHandler<SubCategory> 
        , ISubCategoryHandler
    {
        private readonly IRepository<SubCategory> _Repos;
        public SubCategoryHandler(IRepository<SubCategory> Repos
                                    , ICacheService cache)
            :base(Repos,cache)
        {
            _Repos = Repos;
        }

        IQueryable<SubCategory> ISubCategoryHandler.Get()
        {
            return base.Get().AsQueryable();
            //    return _Repos.TableNoTracking;
        }

    }
}
