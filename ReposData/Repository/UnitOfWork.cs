using System;
using System.Data;
//using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
//using System.Data;
using System.Data.Entity.Core.Objects;
using ReposCore.Extensions;
//using System.Data.Entity.Infrastructure;

namespace ReposData.Repository
{
    public partial interface IUnitOfWork : IDisposable
    {
        Result Commit();
    }
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ReposContext _context;
        private readonly IDbTransaction _transaction;
        private readonly System.Data.Entity.Core.Objects.ObjectContext _objectContext;

        public UnitOfWork(IDbContext context)
        {
            this._context = context as ReposContext ?? new ReposContext();

            this._objectContext = ((IObjectContextAdapter)this._context).ObjectContext;

            if (this._objectContext.Connection.State != ConnectionState.Open)
            {
                this._objectContext.Connection.Open();
            }

            this._transaction = _objectContext.Connection.BeginTransaction();
        }

        public Result Commit()
        {
            Result res = Result.Success();  //  new Result<string>(string.Empty, true, string.Empty);
            try
            {
                this._context.SaveChanges();
                this._transaction.Commit();
            }
            catch (Exception ex)
            {
                Rollback();

                var inex = ex;

                do
                {
                    if (inex.InnerException != null)
                        inex = inex.InnerException;
                    else
                        break;

                } while (true);

                        res = Result.Fail(inex.Message);
                //  throw;
            }

            return res;

        }

        private void Rollback()
        {
            this._transaction.Rollback();

            foreach (var entry in this._context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case System.Data.Entity.EntityState.Modified:
                        entry.State = System.Data.Entity.EntityState.Unchanged;
                        break;
                    case System.Data.Entity.EntityState.Added:
                        entry.State = System.Data.Entity.EntityState.Detached;
                        break;
                    case System.Data.Entity.EntityState.Deleted:
                        entry.State = System.Data.Entity.EntityState.Unchanged;
                        break;
                }
            }
        }

        public void Dispose()
        {
            if (this._objectContext.Connection.State == ConnectionState.Open)
            {
                this._objectContext.Connection.Close();
            }
        }
    }
}

