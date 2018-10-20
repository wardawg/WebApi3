using ReposDomain.Domain;
using ReposServiceConfigurations.ServiceTypes.Base;
using System.Linq;

namespace ReposServices.SubCategoriesServices
{
    public partial interface ISubCategoryTypesService  :IBaseService<SubCategoryType>
   
    {
        SubCategoryType GetBySubCategoryId(int Id);
    }
}
