using Repos.DomainModel.Interface.Interfaces;

namespace ReposServiceConfigures.ServiceTypes.Rules
{
    public interface IViewRule
    {
        
    }
    public interface IViewRule<T> : IViewRule
        where T : BaseEntity
    {
    }
}
