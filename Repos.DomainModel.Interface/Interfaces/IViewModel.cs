namespace Repos.DomainModel.Interface.Interfaces
{
    public interface IViewModel{
    }

    public interface IViewModel<T> : IViewModel
        where T : BaseEntity<T>
    {

    }
        
}
