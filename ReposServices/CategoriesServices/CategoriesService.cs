using ReposData.Repository;
using ReposDomain.Domain;
using RepoServices.CustomerServices;
using ReposServiceConfigurations.ServiceTypes.Base;
using ReposServiceConfigurations.ServiceTypes.Edits;
using ReposServiceConfigurations.ServiceTypes.Rules;

namespace ReposServices.CustomerServices
{
    public class CategoriesService 
        : BaseService<Category>,  ICategoriesService 
    {
      
      
        public CategoriesService(IRepository<Category> categoriesRepository
                                , ICacheService Cache
                                , IDomainEdit Edits
                                , IRule RulesFactory)  
            :base(categoriesRepository,Cache,null, RulesFactory)
        {

        }
             
    }
}
