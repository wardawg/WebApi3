namespace ReposHandlers.Handlers
{
    public class ServiceHandlerFactory : IServiceHandlerFactory
    {
        public T Using<T>() where T : class
        {
            return ServiceHandler.Using<T>();
        }
    }
}
