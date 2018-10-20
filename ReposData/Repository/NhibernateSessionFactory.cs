using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using NHibernate;
using ProjectDependResolver;
using Repos.DomainModel.Interface.Atrributes.DomainAttributes;
using Repos.DomainModel.Interface.Interfaces;
using ReposCore.Infrastructure;
using ReposData.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TestReposDomain.Domain;
using TestReposDomain.Maps;
using FluentNHibernate.Conventions.Instances;

namespace TestReposDomain.Repository
{


    public class NhibernateSessionFactory  //: INhibernateSessionFactory
    {
        private static ISessionFactory _sessionFactory;


        static AutoPersistenceModel CreateAutomappings()
        {

            var maps = new IEnumerable<EntityMapType>[]
                          {  GetMap<IEquipDomainMap>()
                          }.SelectMany(s => s);




            var t2 = AutoMap
                    .Assembly(maps
                    .First()
                    .EntityType
                    .BaseType
                    .GenericTypeArguments
                    .First()
                    .Assembly)
                    .Where(w => !w.IsGenericType); // !w.IsGenericType);

            //.Where(w => w.IsAssignableFrom(typeof(IEquipDomainMap)));

            return t2;


            //return AutoMap.Assemblies(
            //                           new[] { maps
            //                                    .First()
            //                                    .EntityType
            //                                    .BaseType
            //                                    .GenericTypeArguments
            //                                    .First()
            //                                    .Assembly
            //                                    .GetTypes()
            //                                    .Where(s=> s.IsAssignableFrom(typeof(IReposEntityType)))
            //                      });

        }

        static readonly object factorylock = new object();

        public static ISession OpenSession()
        {
            var cfgx = Fluently.Configure()
                  .Database(
                  SQLiteConfiguration.Standard.ConnectionString(
                  c => c.FromConnectionStringWithKey("ReposContext")))
                  .Mappings(x => x.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
                  //.BuildSessionFactory();
                  ;
            _sessionFactory = cfgx.BuildSessionFactory();

            return _sessionFactory.OpenSession();


            lock (factorylock)
            {
                if (_sessionFactory == null)
                {
                    var cfg = Fluently.Configure().
                        Database(SQLiteConfiguration
                                    .Standard
                                    .ShowSql()
                                   // .ConnectionString("C:\\Users\\wardawg-dev\\Documents\\Visual Studio 2015\\Projects\\TestRepos\\TestRepos\\App_Data\\Equipments.db")

                                   .UsingFile("C:\\Users\\wardawg-dev\\Documents\\Visual Studio 2015\\Projects\\TestRepos\\TestRepos\\App_Data\\Equipments.db"))
                                    // .Mappings(m => m.AutoMappings.Add(CreateAutomappings)

                                    .Mappings(val => val
                                                    .AutoMappings
                                                    .Add(
                                        AutoMap.AssemblyOf<WebApiControllers>()
                                        .Where(w => w.Name == "WebApiControllers"))

                        //  .WriteMappingsTo("c:\\temp\\mapping.txt")



                        //(s=> s.Name=="WebApiControllers")
                        //          ))


                        // .ExportTo("c:\\temp")


                        )

                        ;

                    //.AddFromAssemblyOf<MappingsPersistenceModel>());
                    _sessionFactory = cfg.BuildSessionFactory();
                    // BuildSchema(cfg);
                }
                return _sessionFactory.OpenSession();
            }
        }


        public class TableNameConvention : IClassConvention
        {
            public void Apply(IClassInstance instance)
            {
                var schema = instance.EntityType.Namespace.Split('.').Last();
                var typeName = instance.EntityType.Name;
                var tableName = default(string);


                instance.Table(tableName);
            }

        }

        public class AutomappingConfiguration : DefaultAutomappingConfiguration
        {
            public override bool ShouldMap(Type type)
            {
                return type.IsSubclassOf(typeof(testbase));
            }
        }

        private static void BuildSchema(FluentConfiguration configuration)
        {
            var sessionSource = new SessionSource(configuration);
            var session = sessionSource.CreateSession();
            sessionSource.BuildSchema(session);
        }

        private static IEnumerable<EntityMapType> GetMap<T>()
        {

            var interfaceName = typeof(T).Name;

            var maps = EngineContext
                                  .Current
                                  .ContainerManager
                                  .Resolve<ITypeFinder>()
                                  .FindClassesOfType<T>()
                                  .Where(w => !w.GetCustomAttributes(typeof(DomainNoBindAttribute), true)
                                         .Any() && !w.ContainsGenericParameters)
                                  .Select(s => new EntityMapType
                                  {
                                      EntityTypeName = interfaceName
                                      ,
                                      EntityType = s
                                      ,
                                      InheritNumber = GetTypeInheritsNumber(s)
                                  });


            var EntityTypes = maps
                               .Where(w => w.InheritNumber ==
                                       maps
                                          .Where(w2 => w2.EntityType.Name == w.EntityType.Name)
                                          .Min(m => m.InheritNumber)); // base types only

            return EntityTypes;
        }


        private static int GetTypeInheritsNumber(Type t)
        {
            Type tempType = t;
            int typeOrder = 0;

            do


                if (tempType.BaseType != null && tempType.BaseType != typeof(Object))
                {
                    typeOrder++;
                    if (tempType.BaseType.IsGenericType)
                        break;
                    else
                        tempType = tempType.BaseType;
                }
                else
                    break;

            while (true);

            return typeOrder;
        }
    }
}
