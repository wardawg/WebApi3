using Repos.DomainModel.Interface.Interfaces;
using ReposDomain;

namespace ReposServiceConfigurations.ServiceTypes.Edits
{
    public interface IDomainEdit
    {
        IServiceEntityEdit<E> CreateEdit<E>() where E : BaseEntity<E>;
    }
    public interface IDomainEdit<T> 
        : IDomainEdit
        , IReposDomain<T>
    {
    }
}