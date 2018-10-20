using Repos.DomainModel.Interface.Interfaces.Filter;
using System.Linq;

namespace Repos.DomainModel.Interface.Interfaces
{
    public interface IHandler{
    }

    public interface IHandler<T> 
        : IHandler
    {
        IQueryable<T> Get();

        IQueryable<T> Get<TFilter>()
          where TFilter : IFilter;

        IFilter GetFilter<TFilter>()
            where TFilter : IFilter;

    }
}
