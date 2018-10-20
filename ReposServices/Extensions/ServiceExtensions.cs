using Repos.DomainModel.Interface.Interfaces;
using System.Linq;
using System;
using System.Linq.Expressions;
using System.Data.Entity;

namespace ReposServices.Extensions
{
    public static class ServiceExtensions
    {
        public static IQueryable<T> Expand<T, TProperty>(this IQueryable<T> query
                          , Expression<Func<T, TProperty>> path) where T : BaseEntity<T>
        {
            // return null;
            return query.Include(path);
        }

    }
}
