using ProjectDependResolver;

namespace DependencyResolver
{
      

    public interface IConfigOptions
    {
      //  dynamic ConfigOptions { get; }
        void Add(enumConfigOpts EnuOption);
        bool Remove(enumConfigOpts EnuOption);
        bool Contains(enumConfigOpts option);
    }
}
