using Repos.DomainModel.Interface.Interfaces;
using System.Data.Entity.ModelConfiguration;

namespace Repos.DomainModel.Interface
{
    public sealed class ReposEntityHelperTypeConfiguration<TEntity>
            : ReposEntityTypeConfiguration<TEntity>
            where TEntity : BaseEntity<TEntity>
    {
        public ReposEntityHelperTypeConfiguration()
            :base(){
        }

        public ReposEntityHelperTypeConfiguration(string sPrimaryKeyColumn)
          :this()
        {
            this.Property(m => m.Id)
                .HasColumnName(sPrimaryKeyColumn);
        }
    }
}
