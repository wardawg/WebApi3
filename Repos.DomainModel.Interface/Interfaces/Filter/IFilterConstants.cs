namespace Repos.DomainModel.Interface.Interfaces.Filter
{
    public interface IFilterConstants{
    }

    public interface IFilterConstants<T> : IFilterConstants
        where T : new()
    {
        T Filters { get; }
    }
}
