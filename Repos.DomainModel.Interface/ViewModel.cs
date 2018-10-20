using ReposDomain;
using System.Linq;
using System.Web.Mvc;

namespace Repos.DomainModel.Interface
{
    public abstract class ViewModel { }

    public abstract class ViewModel<T>
        : // BaseViewModel<T>
         ,IViewModel<T>
        ,IBaseViewModelRule
        
    where T : BaseEntity<T>
    {
               
        void IBaseViewModelRule.ValidModelRules(dynamic RuleFactory, ModelStateDictionary modelstate)
        {
            //var entity = default(T);

            var entity = this.ToEntity(); //    default(T); // ((IBaseViewModelRule<T>)this).ToEntity();


            var name = GetEntityNameFromModel();

            var Rule = RuleFactory.GetDomainRule(name);
            if (Rule != null) /* dynamic */
                Rule.RunRulesOnModel(entity, modelstate);
        }

        void IBaseViewModelRule.SetViewModelRules(dynamic RuleFactory)
        {

            //   var entity = default(T); // this.ToEntity();
            //    if (entity == null)
            //        return;

            var name = GetEntityNameFromModel();
            var Rule = RuleFactory.GetDomainRule(name);

            if (Rule != null) /* dynamic */
                Rule.SetViewModelRules(this);

           // this.SetCleanViewModel();
        }

        private string GetEntityNameFromModel()
        {
            return
                this.GetType()
                    .BaseType
                    .GetGenericArguments()
                    .FirstOrDefault().Name;
        }
    }
}
