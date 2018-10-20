using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDependResolver
{
    public class ConfigOption
    {
        private string Key { set; get; }
        private object Value { set; get; }
        public ConfigOption(string key,object value)
        {
            Key = key;
            Value = value;
        }
    }
}
