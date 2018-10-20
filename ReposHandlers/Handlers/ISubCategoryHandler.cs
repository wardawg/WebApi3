using Repos.DomainModel.Interface.Interfaces;
using ReposDomain.Domain;
using System.Linq;

namespace ReposDomain.Handlers.Handlers
{
    public interface ISubCategoryHandler : IServiceHandler
    {
        //  IQueryable<SubCategory> Get();
        IQueryable<SubCategory> Get();
    }
}
