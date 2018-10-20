namespace Repos.DomainModel.Interface.Interfaces
{
    public interface IServiceHandlerFactory
    {
        T Using<T>() where T : class, IHandler;

        T Using<T>(IClientInfo Client)
            where T : class, IHandler;
    }
}
