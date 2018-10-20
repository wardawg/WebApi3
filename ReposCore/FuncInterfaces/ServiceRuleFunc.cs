using System;

namespace ReposCore.FunctionInterfaces
{
    public class ServiceRuleFunc<T>  where T : struct
    {
        private Func<object,object,object,T> getRulesValue;
      
        public ServiceRuleFunc(Func<object,object,object,T> func)
        {
            getRulesValue = func;
        }
        public T ExecRules(object Entities, object ModeState,object RuleFactory)
        {
            return getRulesValue(Entities, ModeState, RuleFactory);
        }      
      
    }
}
