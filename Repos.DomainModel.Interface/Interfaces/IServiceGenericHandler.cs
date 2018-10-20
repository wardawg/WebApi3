using System.Collections.Generic;
using System.Linq;

namespace Repos.DomainModel.Interface.Interfaces
{
    public interface IServiceGenericHandler 
        : IGenericHandler
         
    {
        IQueryable GetData<IDataEntity>()
              where IDataEntity : IGenericHandler;
        
    }

    public interface IServiceGenericHandler<T> 
        : IServiceGenericHandler
    {
         List<T> GetData();
            
    }
}
