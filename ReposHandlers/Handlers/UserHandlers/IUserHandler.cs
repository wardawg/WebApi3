using Repos.DomainModel.Interface.Interfaces;
using System.Linq;

namespace ReposDomain.Handlers.Handlers.UsersHandlers
{
    public interface IUserHandler : IServiceHandler
    {
        bool IsAdmin();

        IQueryable GetUserRoles(int Id);
        IQueryable GetUserRules(int Id);
    }
}
