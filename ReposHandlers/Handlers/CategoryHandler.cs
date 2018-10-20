using Repos.DomainModel.Interface;
using ReposCore.Caching;
using ReposData.Repository;
using ReposDomain.Domain;
using ReposServiceConfigurations.ServiceTypes.Base;
using ReposServiceConfigures.ServiceTypes.Handlers;

namespace ReposDomain.Handlers.Handlers
{

    public class CategoryHandler 
        : ServiceGenericHandler<Category>
          ,ICategoryHandler
    {
        public CategoryHandler(IRepository<Category> repos
                                , ICacheService cache)
            :base(repos,cache){
        }
    }
}
