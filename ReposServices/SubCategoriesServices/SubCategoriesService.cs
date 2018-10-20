using ReposData.Repository;
using ReposDomain.Domain;
using ReposServiceConfigurations.ServiceTypes.Base;
using ReposServiceConfigurations.ServiceTypes.Edits;
using ReposServiceConfigurations.ServiceTypes.Rules;
using ReposServiceConfigurations.ServiceTypes.Base;

namespace ReposServices.SubCategoriesServices
{
    public class SubCategoriesService
        : BaseService<SubCategory>, ISubCategoriesService
    {
        
       
        public SubCategoriesService(IRepository<SubCategory> subCategoryRepository
                                    , ICacheService Cache
                                    , IDomainEdit Edits
                                    , IRule Rules
                                    )
                                         
        :base(subCategoryRepository,Cache, Edits,Rules){
         }
              
        
    }       
}
