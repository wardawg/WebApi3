using Repos.DomainModel.Interface.Interfaces;
using ReposData.Repository;
using ReposDomain.Domain;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace ReposDomain.Handlers.Handlers
{


 
    public class SubCategoryUserLevelSearch
    {
        public int SubCategoryLevelId { get; set; }
        public int SubCategoryId { get; set; }
        public int UserId { get; set; }
    }


    public interface ISubCategoryUserLevelHandler : IServiceHandler
    {
        IQueryable<SubCategoryLevel> GetUserCatLevels(User user,Expression<Func<SubCategoryLevel, bool>> Level = null
                                                    );

    }

   


    public class SubCategoryUserLevelHandler : ISubCategoryUserLevelHandler
    {
        private readonly IRepository<SubCategoryLevel> _Repos_SubCategoryLevel;
        private readonly IRepository<SubCategoryUserLevel> _Repos_SubCategoryUserLevel;


        public SubCategoryUserLevelHandler(IRepository<SubCategoryLevel> Repos_SubCategoryLevel
                                          ,IRepository<SubCategoryUserLevel> Repos_SubCategoryUserLevel)
        {
            _Repos_SubCategoryLevel = Repos_SubCategoryLevel;
            _Repos_SubCategoryUserLevel = Repos_SubCategoryUserLevel;
        }

        public IQueryable<SubCategoryLevel> GetUserCatLevels(User user,Expression<Func<SubCategoryLevel,  bool>> Level = null)
        {

            Func<SubCategoryUserLevel, bool> deg = i => true;


            //         Expression<Func<SubCategoryLevel, bool>> GetOrExpression(
            //Expression<Func<SubCategoryLevel, bool>> e1,
            //Expression<Func<SubCategoryUserLevel, bool>> e2)
            //         {
            //             return s => e1.Compile()(s); // || e2.Compile()(null);
            //         };


            int busr = (user == null && user.Id == 0) ? 0 : user.Id;


            Expression<Func<SubCategoryLevel, bool>> expr = i => true;
            Expression<Func<SubCategoryUserLevel, bool>> expr2;  // = i => busr !=0 ? i.Id == busr : true;

            var f1 = Level == null ? expr : Level;
            var f2 = busr != 0 ? expr2 = i => i.Id == busr : expr2 = i => true;


            var res = from scl  in _Repos_SubCategoryLevel.TableNoTracking.Where(f1)
                      from scul in _Repos_SubCategoryUserLevel.TableNoTracking.Where(f2)
                      where
                           scl.Id == scul.SubCategoryLevelId
                       select scl;
                     ;

            return res;
        }
        
    }



}
