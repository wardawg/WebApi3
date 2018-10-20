using Repos.DomainModel.Interface;
using Repos.DomainModel.Interface.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace ReposDomain.Handlers.Extensions
{
    public static class HandlerExensions
    {
        public static IQueryable<T> Expand<T, TProperty>(this IQueryable<T> query
                           , Expression<Func<T, TProperty>> path) where T : BaseEntity<T>
        {
           // return null;
            return query.Include(path);
        }
    }
}
