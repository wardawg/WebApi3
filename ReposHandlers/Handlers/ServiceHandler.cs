using ReposCore.Infrastructure;
using System;

namespace ReposHandlers.Handlers
{
    public class ServiceHandler :IServiceHandler
    {
        private static IEngine HandlerResolve = EngineContext.Current;
        
        public static T Using<T>() where T : class
        {

            T handler = default(T);
                      

           
            try
            {
                if (HandlerResolve == null)
                    throw new Exception("Current Context for resolving handler is not defined");
               

                if (!typeof(T).IsInterface)
                     throw new Exception("Expection type: " + typeof(T).Name + " to a be interface");
               

                handler = HandlerResolve.Resolve<T>(); 
                if (handler == null)
                {
                    throw new NullReferenceException("Unable to resolve type with service locator; type " + typeof(T).Name);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return handler;
        }
        
     }
 }
