//using RepositoryPattern_RepoInterfaces;
using ReposData.Repository;
using ReposDomain.Domain;
using ReposServiceConfigurations.ServiceTypes.Base;
using ReposServiceConfigurations.ServiceTypes.Edits;


namespace RepoServices.CustomerServices
{
    public class CustomerService 
        : BaseService<Customer>, ICustomerService

    {
      
        public CustomerService(IRepository<Customer> customerRepository
                             , ICacheService Cache
                              , IDomainEdit Edits) 
            :base(customerRepository,Cache, Edits,null){
        }

         
    }
}
