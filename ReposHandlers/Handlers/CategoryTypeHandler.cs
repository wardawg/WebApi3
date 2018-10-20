using ReposCore.Caching;
using ReposData.Repository;
using ReposDomain.Domain;
using ReposServiceConfigurations.ServiceTypes.Base;
using ReposServiceConfigures.ServiceTypes.Handlers;

namespace ReposDomain.Handlers.Handlers
{

    public class CategoryTypeHandler
        : ServiceGenericHandler<CategoryType>
        ,ICategoryTypeHandler
    {
        public CategoryTypeHandler(IRepository<CategoryType> repos
                                    , ICacheService cache)
            :base(repos,cache)
        {

        }
               
    }

      
}  
