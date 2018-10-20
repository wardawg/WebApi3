using Repos.DomainModel.Interface.Interfaces;
using ReposData.Repository;
using ReposDomain.Domain;
using System.Linq;

namespace ReposDomain.Handlers.Handlers
{
    public interface ISubCategoryLevelHandler : IServiceHandler
    {
        IQueryable<SubCategoryLevel> Get();
    }


    public class SubCategoryLevelHandler : ISubCategoryLevelHandler
    {
        private readonly IRepository<SubCategoryLevel> _Repos;
        public SubCategoryLevelHandler(IRepository<SubCategoryLevel> Repos)
        {
            _Repos = Repos;
        }

        public IQueryable<SubCategoryLevel> Get()
        {
            return _Repos.TableNoTracking;
        }
                
    }



}
