using Repos.DomainModel.Interface.Interfaces;
using System;

namespace Repos.DomainModel.Interface.Common
{       
    public class DefaultClientInfo : 
        IClientInfo<DefaultClientInfo>
    {
        private string _AssmPrefix = "ReposDomain";
        private string _ExtClientId = "$_$_$_$_";
        public Enum FilterEnum { get; set; }

        public DefaultClientInfo(){
            SetInfo();
        }

        
        public DefaultClientInfo(int InId
                                ,string AssmPrefix
                                ,string ExtClientId)
        {
            Id = InId;
            _AssmPrefix = AssmPrefix;
            _ExtClientId = ExtClientId;
        }

        protected virtual void SetInfo()
        {
            _AssmPrefix = DefaultPrefix;
        }

        public dynamic GetEnum(string enumName)
        {
            return FilterEnum;
        }

        public string AssmPrefix => _AssmPrefix;
        public string ExtClientId => _ExtClientId;
        public string DefaultPrefix { get => "ReposDomain"; }
        public int Id { internal set; get; }
       
        public dynamic AsClientFilter<T>(object c)         {
            return (T)Enum.Parse(typeof(T), c.ToString(), false);
        }

    }
}
