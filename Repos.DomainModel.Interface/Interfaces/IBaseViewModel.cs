namespace Repos.DomainModel.Interface.Interfaces
{
    // implement 
    // on base view model
    public interface IBaseViewModelRule
    {
        void ValidModelRules(dynamic RuleFactory, dynamic modelstate,IClientInfo client);

        void SetViewModelRules(dynamic RuleFactory, IClientInfo client, bool cleanState);
    }


}
