namespace Repos.DomainModel.Interface.Interfaces.DomainList
{
    public class DefaultListFactory : IListFactory
    {
        public T Using<T>() where T : class
        {
            return default(T);
        }
    }
}
