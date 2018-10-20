using Repos.DomainModel.Interface.Interfaces;
using ReposCore.Extensions;
using ReposServiceConfiguration.ServiceTypes.Base;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace ReposServiceConfigurations.ServiceTypes.Base
{

    public interface IBaseService{
        
    }
    public interface IBaseService<T> 
        : IBaseService 
        where T : BaseEntity<T>
    {
        void Add(T Entity);
        void Update(T Entity);
        void Update(T Entity, IClientInfo client);
        Result Delete(T Entity);
        Result Verify(ModelStateDictionary ModelState);
        Result Verify(ModelStateDictionary ModelState, IClientInfo clientInfo);

        T GetById(object Id);
        IQueryable<T> GetAll();
        T CreateServiceEntity();
        T CreateServiceEntity(IClientInfo clientInfo);
        T CreateServiceEntity(CreateEntityOptions createEntityOptions);
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        
    }
}
