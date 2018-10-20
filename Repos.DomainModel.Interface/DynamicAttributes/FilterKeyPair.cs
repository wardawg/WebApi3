using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos.DomainModel.Interface.DynamicAttributes
{
    public class FilterKeyPair
        : AttributeList
    {

        public FilterKeyPair()
        {
        }

        public string Value { set; get; }
        public List<string> Filters { set; get; } = new List<string>();


        public FilterKeyPair(string value, List<string> filters)
        {
            Value = value;
            Filters = filters;
        }
    }
}
