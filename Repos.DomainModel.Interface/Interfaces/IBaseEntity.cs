namespace Repos.DomainModel.Interface.Interfaces
{
    
    public interface IDomainRepository{
    }

    public interface IEntity{
    }

    public interface IBaseEntity : IEntity { 
        bool RulesEnabled { set; get; }
        string ObjName { get;}
    }

    public interface IBaseEntity<T> : IBaseEntity
    { 
       
        T OldPicture { get; set; }
        
    }
}
