using Repos.DomainModel.Interface.Interfaces;

namespace Repos.DomainModel.Interface.DomainViewModels
{
    public interface IDomainViewModel{
    }

    public interface IDomainViewModel<T> : IDomainViewModel
        where T : BaseEntity<T>
    {

    }
        
}
