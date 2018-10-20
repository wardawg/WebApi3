using Repos.DomainModel.Interface.Interfaces;
using ReposData.Repository;
using ReposDomain.Domain;
using ReposDomain.Handlers.Extensions;
using ReposDomain.Handlers.Handlers.UsersHandlers;
using System.Linq;

namespace ReposDomain.Handlers.Handlers.UsersHandlers
{

    public interface IUserHandler : IServiceHandler
    {
        bool IsAdmin();

        IQueryable GetUserRoles(int Id);
        IQueryable GetUserRules(int Id);
    }


    public class UserHandler 
        : IUserHandler
    {
        private readonly IRepository<User> _UserRepos;
        private readonly IRepository<UserRule> _UserRule;
        private readonly IRepository<UserRole> _UserRole;
        public UserHandler(IRepository<User> UserRepos
                           ,IRepository<UserRule> UserRule
                           ,IRepository<UserRole> UserRole)
        {
            _UserRepos = UserRepos;
            _UserRule = UserRule;
            _UserRole = UserRole;
    }

        public IQueryable GetUserRoles(int Id)
        {



            var results = from ur in _UserRole.TableNoTracking
                           let usr = ur.User
                           where ur.User.Id == Id
                           select new
                           {
                               ur.Id
                               ,
                               RoleId = ur.Role.Id
                               ,
                               ur.Role.RoleName

                           };


            return results;

        }

        public IQueryable GetUserRules(int Id)
        {

                    
            var results = from ur in _UserRule.TableNoTracking
                          where ur.Id == Id
                          let rule = ur.Rule
                          select new
                          {
                               ur.Id
                              ,RuleId = rule.Id
                              ,rule.RuleName
                          };

            return results;
        }

        public bool IsAdmin()
        { 
            return true; // _user.Admin == 1;
        }
    }
}
