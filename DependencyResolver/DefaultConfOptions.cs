using ProjectDependResolver;
using System.Collections.Generic;

namespace ProjectDependResolver
{
   
    public class DefaultConfOptions
        : ConfOptions
    {
        protected List<enumConfigOpts> Ops = new List<enumConfigOpts>();

       public override enumConfigOpts ConfigOptions =>  new enumConfigOpts();

        public override void Add(enumConfigOpts EnuOption)
        {
            Ops.Add(EnuOption);
        }

        public override bool Remove(enumConfigOpts EnuOption)
        {
            return Ops.Remove(EnuOption);
        }

        public override bool Contains(enumConfigOpts option)
        {
            var ret = Ops.Contains(option);

            return ret;
        
        }

        
    }
}
