using Repos.DomainModel.Interface.Interfaces;

namespace ReposServiceConfigurations.ServiceTypes.Edits
{


    public abstract class DomainEdit 
        : IDomainEdit
    {
     
        public abstract void RunEdits(IBaseEntity Entity);
        public abstract void SetEntitiesDefaults(IBaseEntity Entity);
        public abstract void SetEntitiesProps(IBaseEntity Entity);
        public abstract void SetEntitiesFilters(IBaseEntity Entity);

        public abstract IServiceEntityEdit<T> CreateEdit<T>() where T : BaseEntity<T>;
        protected bool AddDomainAttributes { set; get; } = true;
        
    }
}
