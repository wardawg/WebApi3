using Repos.DomainModel.Interface.Atrributes.DomainAttributes;
using Repos.DomainModel.Interface.Interfaces;
using ReposCore.Infrastructure;
using ReposServiceConfigurations.Common;
using ReposServiceConfigurations.ServiceTypes.Enums;
using System;

namespace ReposServiceConfigures.ServiceTypes.Handlers
{
    [DomainNoBindAttribute]
    public sealed class ServiceHandler :IServiceHandler
    {
        static IEngine HandlerResolve = EngineContext.Current;

        private T CreateFilter<T>()
            where T : IHandler, new()
        {
            return new T();
        }

        private static T Using<T>(Type t) where T : class,IHandler
        {

          
            var typeNameResolve = CommonUtil
                                    .GetResolveName(t
                                                   , Name:string.Empty
                                                   , postFix: EnumServiceTypes.Handlers
                                                   );

            
            return EngineContext
                   .Current
                   .ContainerManager
                   .Resolve<IHandler>(typeNameResolve) as T;

        }

        public static T Using<T>() where T : class,IHandler
        {

            T handler = default(T);
                      

           
            try
            {
                if (HandlerResolve == null)
                    throw new Exception("Current Context for resolving handler is not defined");
               
                
                if (typeof(T) == typeof(IServiceGenericHandler))
                    handler = HandlerResolve.Resolve<T>(AllowNull: true);
                else
                    handler = Using<T>(typeof(T)); 

                if (handler == null)
                {
                    throw new NullReferenceException("Unable to resolve type with service locator; type " + typeof(T).Name);
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return handler;
        }

        private static T GetFilterByName<T>(string filterName)
            where T : class, IServiceHandler
        {
            return EngineContext
                   .Current
                   .ContainerManager
                   .Resolve<T>(key: filterName, AllowNull:true);
        }
    }
 }
