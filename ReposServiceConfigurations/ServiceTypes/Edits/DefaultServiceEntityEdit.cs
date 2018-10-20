using Repos.DomainModel.Interface.Interfaces;
using ReposServiceConfigurations.ServiceTypes.Edits;

namespace ReposServiceConfigurations.ServiceTypes.Base
{
    public sealed class DefaultServiceEntityEdit
        : IDomainEdit

    {
        public IServiceEntityEdit<T> CreateEdit<T>() where T : BaseEntity<T>
        {
            return null;
        }
    }
}
