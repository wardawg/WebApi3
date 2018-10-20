using Repos.DomainModel.Interface.Interfaces;
using ReposCore.Caching;
using ReposServiceConfigurations.ServiceTypes.Base;

namespace ReposDomain.Handlers.Base
{
    public abstract class BaseHandler
    {

        private ICacheService _cache;
        readonly IServiceHandler _BaseRuleHandler;


        public ICacheService Cache() => _cache;
                  
        public BaseHandler(IServiceHandler BaseRunHandler
                           , ICacheService cache)
        {
            _BaseRuleHandler = BaseRunHandler;
            _cache = cache;

        }
    }
}
