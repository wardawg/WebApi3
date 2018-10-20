using Repos.DomainModel.Interface.Interfaces;
using ReposCore.Infrastructure;
using ReposServiceConfigurations.ServiceTypes.Enums;

namespace ReposServiceConfigures.ServiceTypes.Handlers
{
    public class ServiceHandlerFactory : IServiceHandlerFactory
    {
        public T Using<T>() 
            where T : class, IHandler
        {
            return ServiceHandler.Using<T>();
        }

        public T Using<T>(IClientInfo clientInfo)
            where T : class, IHandler
        {

            var HandlerName = typeof(T).Name.Substring(1);

            string ClientPrefix = clientInfo.AssmPrefix;
            string DefaultPrefix = clientInfo.DefaultPrefix;
            string ResolvefilterName;

           // if (clientInfo.AssmPrefix != clientInfo.DefaultPrefix)
                ResolvefilterName = string.Format("{0}.{1}.", ClientPrefix, EnumServiceTypes.Handlers) + HandlerName;
           
            var exists = EngineContext
                       .Current
                       .ContainerManager.IsRegisteredByName(ResolvefilterName, typeof(IHandler));

            if (!exists)
            {
                ResolvefilterName = string.Format("{0}.{1}.", DefaultPrefix, EnumServiceTypes.Handlers) + HandlerName;
            }
            return EngineContext
                  .Current
                  .ContainerManager
                  .Resolve<IHandler>(ResolvefilterName) as T;
            
        }
    }
}
