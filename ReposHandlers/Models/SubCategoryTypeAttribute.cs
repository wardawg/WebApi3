using Repos.DomainModel.Interface;
using Repos.DomainModel.Interface.Atrributes.DomainAttributes;
using Repos.DomainModel.Interface.Interfaces;
using ReposCore.Caching;
using ReposData.Repository;
using ReposServiceConfigurations.ServiceTypes.Base;
using ReposServiceConfigures.ServiceTypes.Handlers;

namespace ReposDomain.Handlers.Models
{

    public interface ISubCategoryTypeAttribute : IGenericHandler{
    }
    
    [DomainNoBindAttribute]
    public class SubCategoryTypeAttribute
     : ServiceGenericHandler<SubCategoryTypeAttribute> , ISubCategoryTypeAttribute
    {
        public string AttributeName { set; get; }
        
        public SubCategoryTypeAttribute(IRepository<SubCategoryTypeAttribute> repos
                                        , ICacheService cache)
               : base(repos,cache){
        }

    }
}