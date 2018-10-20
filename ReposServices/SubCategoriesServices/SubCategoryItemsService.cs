using ReposData.Repository;
using ReposDomain.Domain;
using ReposServiceConfigurations.ServiceTypes.Base;
using ReposServiceConfigurations.ServiceTypes.Edits;
using ReposServiceConfigurations.ServiceTypes.Base;
using ReposServices.SubCategoriesServices;

namespace ReposServices.SubCategorieServices
{
    public partial class SubCategoryItemsService 
        : BaseService<SubCategoryItem>, ISubCategoryItemsService
    {
               
        public SubCategoryItemsService(IRepository<SubCategoryItem> subCategoryItemsRepository
                                     , ICacheService Cache
                                     , IDomainEdit Edits
            )  :base(subCategoryItemsRepository,Cache, Edits,null){
                }
                       
    }
}
