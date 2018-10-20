using Repos.DomainModel.Interface.DomainComplexTypes;
using Repos.DomainModel.Interface.Interfaces;
using System;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Repos.DomainModel.Interface
{
    public abstract class ReposEntityTypeConfiguration<T> 
        : EntityTypeConfiguration<T> 
        , IReposEntityType
        where T : BaseEntity<T>
    {
        protected ReposEntityTypeConfiguration()
        {
            PostInitialize();
        }

        protected ReposEntityTypeConfiguration(string sTableNm, string sPrimKeyCol)
            :this()
        {
            SetTableMap(sTableNm, sPrimKeyCol);
        }

        protected virtual void SetTableMap(string sTableNm, string sPrimKeyCol)
        {
           // this.ToTable(sTableNm);
            this.HasKey(m => m.Id);
            this.Property(m => m.Id)
            .HasColumnName(sPrimKeyCol);
        }
        /// <summary>
        /// Developers can override this method in custom partial classes
        /// in order to add some custom initialization code to constructors
        /// </summary>
        protected virtual void PostInitialize()
        {
            MapComplexDomainObects();
        }

               
        private static Expression<Func<TC, U>> BuildLambda<TC, U>(PropertyInfo property)
        {
            var param = Expression.Parameter(typeof(TC), "p"); 
            MemberExpression member = Expression.Property(param, property);
            // Get property of property
            MemberExpression memberField = Expression.PropertyOrField(member, "Value");
            var lambda = Expression.Lambda<Func<TC, U>>(memberField, param);

            return lambda;
        }

        private void MapComplexDomainObects()
        {
            foreach (var prop in typeof(T)
                             .GetProperties(BindingFlags.Public
                                             | BindingFlags.Instance
                                             | BindingFlags.DeclaredOnly)
                            .Where(w => w.PropertyType.BaseType != null
                                     && w.PropertyType.BaseType.IsGenericType
                                     && w.PropertyType
                                        .BaseType
                                        .GetGenericTypeDefinition() == typeof(DomainEntityType<>))
                    )

            {
                dynamic complexType = default(dynamic);
                switch (prop.PropertyType.BaseType.GetGenericArguments()[0].Name.ToLower())
                {
                    case "string":
                        complexType = BuildLambda<T, string>(prop);
                        break;
                    case "int":
                    case "int32":
                        complexType = BuildLambda<T, Int32>(prop);
                        break;
                    case "double":
                        complexType = BuildLambda<T, double>(prop);
                        break;
                    case "float":
                        complexType = BuildLambda<T, float>(prop);
                        break;
                    case "datetime":
                        complexType = BuildLambda<T, DateTime>(prop);
                        break;
                }

                this.Property(complexType)
                    .HasColumnName(prop.Name);
            }

        }
    }
}
