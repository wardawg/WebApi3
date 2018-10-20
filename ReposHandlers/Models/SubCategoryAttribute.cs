using Repos.DomainModel.Interface;
using Repos.DomainModel.Interface.Interfaces;
using ReposCore.Caching;
using ReposData.Repository;
using ReposServiceConfigurations.ServiceTypes.Base;
using ReposServiceConfigures.ServiceTypes.Handlers;

namespace ReposDomain.Handlers.Models
{

    public interface ISubCategoryAttribute : IGenericHandler{
    }

    public class SubCategoryAttribute
       : ServiceGenericHandler<SubCategoryAttribute> 
      , ISubCategoryAttribute
    {
        
        public string AttributeName { set; get; }
                
        public SubCategoryAttribute() { 
}
        public SubCategoryAttribute(IRepository<SubCategoryAttribute> repos
                                    , ICacheService cache)
            :base(repos,cache){
        }
    }
}
