namespace ReposServiceConfigurations.ServiceTypes.Rules
{

    public interface IModelRule
    {

    }
    public interface IViewModelRule : IModelRule
    {
        void AppyModelRules(IModelRule rule);
        void SetViewModelRules(object viewModel);
    }

    public interface IViewModelRule<T>
    {
       
    }
        
}
