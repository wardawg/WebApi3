using System.Configuration;
using System.Runtime.CompilerServices;
using ReposDomain.Infrastructure;

namespace ReposCore.Infrastructure
{
    public class EngineContext
    {
        #region Methods

        /// <summary>
        /// Initializes a static instance of the Nop factory.
        /// </summary>
        /// <param name="forceRecreate">Creates a new factory instance even though the factory has been previously initialized.</param>
        [MethodImpl(MethodImplOptions.Synchronized)]

        public static IEngine Initialize()
        {
            var config = ConfigurationManager.GetSection("ReposConfig") as IConfigurationSectionHandler;
            return Initialize(new ReposEngine(), config);
        }

        //public static IEngine Initialize(IEngine Engine)
        //{
        //    Singleton<IEngine>.Instance = Engine;
        //     var config = ConfigurationManager.GetSection("ReposConfig")  as IConfigurationSectionHandler;
        //     Singleton<IEngine>.Instance.Initialize(config);
            
        //    return Singleton<IEngine>.Instance;
        //}

        public static IEngine Initialize(IEngine Engine, IConfigurationSectionHandler config)
        {
            Singleton<IEngine>.Instance = Engine;
          //  var config = ConfigurationManager.GetSection("ReposConfig") as IConfigurationSectionHandler;
            Singleton<IEngine>.Instance.Initialize(config);

            return Singleton<IEngine>.Instance;
        }

        /// <summary>
        /// Sets the static engine instance to the supplied engine. Use this method to supply your own engine implementation.
        /// </summary>
        /// <param name="engine">The engine to use.</param>
        /// <remarks>Only use this method if you know what you're doing.</remarks>
        public static void Replace(IEngine engine)
        {
            Singleton<IEngine>.Instance = engine;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the singleton Nop engine used to access Nop services.
        /// </summary>
        public static IEngine Current
        {
            get
            {
                if (Singleton<IEngine>.Instance == null)
                {
                    Initialize();
                }
                return Singleton<IEngine>.Instance;
            }
        }

        #endregion
    }
}
