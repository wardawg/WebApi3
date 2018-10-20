using Repos.DomainModel.Interface.Interfaces;

namespace Repos.DomainModel.Interface.DomainViewModels
{

    public abstract class DomainViewModel{
    }
    public abstract class DomainViewModel<T>
        : DomainViewModel
        where T : BaseEntity<T>

    {
                        
       
        
    }
}
