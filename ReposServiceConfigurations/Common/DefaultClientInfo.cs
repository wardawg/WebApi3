using Repos.DomainModel.Interface.Interfaces;
using System;

namespace ReposServiceConfigurations.Common
{
    public class DefaultClientInfo :
        IClientInfo<DefaultClientInfo>
    {
        private string _AssmPrefix = "ReposDomain";
        private string _ExtClientId =  "_$_$_$_$_";
        public Enum FilterEnum { get; set; }

        public DefaultClientInfo() {
            SetInfo();
        }

        public DefaultClientInfo(int inId
                                ,string AssmPrefix
                                ,string ExtClientId
                                ,string clientKey)
        : this()
        {
            Id = inId;
            _AssmPrefix = AssmPrefix;
            _ExtClientId = ExtClientId;
            ClientKey = clientKey;
        }

        protected virtual void SetInfo()
        {
            _AssmPrefix = DefaultPrefix;
            Id = 0;
            if (String.IsNullOrEmpty(DefaultPrefix))
                DefaultPrefix = "ReposDomain";
        }

        public dynamic GetEnum(string enumName)
        {
            return FilterEnum;
        }

        public string AssmPrefix => _AssmPrefix;

       // public string DefaultPrefix { get => "ReposDomain"; }
       public string DefaultPrefix { get; set; }

        public int Id { get; private set; }

        public string ExtClientId => _ExtClientId;
        public string ClientKey { get; private set; }
 
    }
}
