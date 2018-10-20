namespace ReposServiceConfigure.ServiceTypes.Handlers
{
    public interface IServiceHandlerFactory
    {
        T Using<T>() where T : class;

    }
}
