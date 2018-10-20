using Repos.DomainModel.Interface.Interfaces;

namespace ReposServiceConfigurations.Common
{
    public class CommonInfo : ICommonInfo
    {
        

        public CommonInfo()
        {
            setDefaults();
        }
        public virtual void setDefaults()
        {
            ClientInfo = new DefaultClientInfo();
        }
                
        public IClientInfo ClientInfo { set; get; }

        
    }
}
