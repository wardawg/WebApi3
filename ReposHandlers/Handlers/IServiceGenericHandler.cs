using Repos.DomainModel.Interface.Interfaces;
using ReposServiceConfigure.ServiceTypes.Handlers;
using System.Linq;

namespace ReposHandlers.Handlers
{
    public interface IServiceGenericHandler 
        : IServiceHandler
    {
        IQueryable GetData<Domain>()
             where Domain : BaseEntity<Domain>;

    }

    public interface IServiceGenericHandler<T> 
        : IServiceGenericHandler
    {
        
            
    }
}
