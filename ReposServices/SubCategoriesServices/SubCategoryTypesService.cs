using ReposData.Repository;
using ReposDomain.Domain;
using ReposServiceConfigurations.ServiceTypes.Base;
using ReposServiceConfigurations.ServiceTypes.Edits;
using ReposServiceConfigurations.ServiceTypes.Base;
using System.Linq;

namespace ReposServices.SubCategoriesServices
{
    public partial class SubCategoryTypesService 
        : BaseService<SubCategoryType>, ISubCategoryTypesService
    {
        
        public SubCategoryTypesService(IRepository<SubCategoryType> subCategoryTypeRepository
                                    , ICacheService Cache
                                    , IDomainEdit Edits)  
            : base(subCategoryTypeRepository,Cache, Edits,null){
              }


        public  SubCategoryType GetBySubCategoryId(int Id)
        {
           return Where(t => t.SubCategoryId == Id)
                  .FirstOrDefault();

            


        }


    }
}
