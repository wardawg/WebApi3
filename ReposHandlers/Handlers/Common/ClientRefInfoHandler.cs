using Repos.DomainModel.Interface.Interfaces;
using ReposData.Repository;
using ReposDomain.Domain;
using ReposServiceConfigurations.Common;
using System.Collections.Generic;
using System.Linq;

namespace ReposDomain.Handlers.Handlers.Common
{
    
    public interface IClientRefInfoHandler : IServiceHandler
    {
        IQueryable<DefaultClientInfo> Get();
        IQueryable<Client> GetClients();
    }

    public class ClientRefInfoHandler
        : IClientRefInfoHandler
    {

        private readonly IRepository<ClientRefInfo> _Repos;
        private readonly IRepository<Client> _Client;

        public ClientRefInfoHandler(IRepository<ClientRefInfo> Repos
                                    ,IRepository<Client> client)
        {
            _Repos = Repos;
            _Client = client;
        }


        public IQueryable<Client> GetClients()
        {
            return _Client.TableNoTracking;
        }

        public virtual IQueryable<DefaultClientInfo> Get()
        {
            List<DefaultClientInfo> client = new List<DefaultClientInfo>();

                _Repos
                 .TableNoTracking
                 .ToList()
                 .ForEach(f => client.Add(new DefaultClientInfo(f.Id
                                            , f.AssmPrefix
                                            ,f.ExtClientId
                                            ,f.ClientKey)));

            return client.AsQueryable();
        }
    }
}
