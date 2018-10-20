using Repos.DomainModel.Interface.Interfaces;

namespace ReposServiceConfigurations.ServiceTypes.Edits
{
    public interface IServiceEntityEdit
    {
        
    }
    public interface IServiceEntityEdit<T> 
        : IServiceEntityEdit
         where T : BaseEntity<T>
    {
       void RunEdits();
       void SetEntityDefaults(T Entity);
       void SetEntityValues(T Entity);
        
      
    }
}
