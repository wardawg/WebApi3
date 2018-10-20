using Repos.DomainModel.Interface.Interfaces;
using Repos.DomainModel.Interface.Interfaces.DomainList;
using Repos.DomainModel.Interface.Interfaces.Filter;
using ReposCore.Infrastructure;
using System.Linq;

namespace ReposServiceConfigurations.ServiceTypes.Edits
{
    public class ReposDomainEdit{
    }

    public class ReposDomainEdit<T>
         : DomainEdit
        , IDomainEdit

    {

        protected readonly IServiceHandlerFactory _ServiceHandlerFactory;
        protected readonly IFilterFactory _FilterFactory;
        protected readonly IListFactory _ListFactory;
        protected readonly ICommonInfo _CommonInfo;
        public ReposDomainEdit(IServiceHandlerFactory ServiceHandlerFactory
                               , IFilterFactory FilterFactory
                               , IListFactory ListFactory
                               , ICommonInfo CommonInfo
                               )
        {
            _ServiceHandlerFactory = ServiceHandlerFactory;
            _FilterFactory = FilterFactory;
            _ListFactory = ListFactory;
            _CommonInfo = CommonInfo;


        }

        

        protected IClientInfo ClientInfo
        {
            get
            { return _CommonInfo.ClientInfo; }
        }

        public IReposModel<T> Model { set; get; }
        
        public override IServiceEntityEdit<E> CreateEdit<E>()
        {

            var exists = EngineContext
                        .Current
                        .ContainerManager.IsRegistered(typeof(IServiceEntityEdit<E>));


            var t = default(IServiceEntityEdit<E>);

            if (exists)
                t = EngineContext
                          .Current
                          .ContainerManager
                          .Resolve<IServiceEntityEdit<E>>() as IServiceEntityEdit<E>;

            return t;
        }

        public string AssmPrefix()
        {
            return GetType()
                    .Assembly
                    .FullName
                    .Split('.')
                    .FirstOrDefault();

        }

        public override void RunEdits(IBaseEntity Entity){
        }

        public override void SetEntitiesDefaults(IBaseEntity Entity){
        }

        public override void SetEntitiesFilters(IBaseEntity Entity){
        }

        public override void SetEntitiesProps(IBaseEntity Entity){
        }

    }
}
