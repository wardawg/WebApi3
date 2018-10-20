namespace Repos.DomainModel.Interface.Interfaces
{
    public interface IClientInfo
    {
      string AssmPrefix { get;}
      string DefaultPrefix { get; }
      dynamic GetEnum(string enumName);
      int Id { get; }
      string ExtClientId { get; }


    }

    public interface IClientInfo<T>
        : IClientInfo{

        
    }

}
