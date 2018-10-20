using ReposData.Repository;
using ReposDomain.Domain;
using ReposServiceConfigurations.ServiceTypes.Base;
using ReposServiceConfigurations.ServiceTypes.Edits;
using ReposServiceConfigurations.ServiceTypes.Rules;
using ReposServiceConfigurations.ServiceTypes.Base;

namespace ReposServices.SubCategoriesServices
{
    public partial class SubCategoryClassItemService
         : BaseService<SubCategoryClassItem>, ISubCategoryClassItemsService
    {
        public SubCategoryClassItemService(IRepository<SubCategoryClassItem> Repos
                                          , ICacheService Cache
                                          , IDomainEdit Edits
                                          , IRule Rules) 
            : base(Repos,Cache, Edits, Rules)
        {
        }
    }
}
