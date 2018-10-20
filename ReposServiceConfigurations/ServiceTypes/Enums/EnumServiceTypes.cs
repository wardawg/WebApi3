using ReposCore.Common;
using System.Collections.Generic;

namespace ReposServiceConfigurations.ServiceTypes.Enums
{
    public class EnumServiceTypes : Enumeration
    {
        public static EnumServiceTypes None = new EnumServiceTypes(1, string.Empty);
        public static EnumServiceTypes Common = new EnumServiceTypes(2, "Common");
        public static EnumServiceTypes Handlers = new EnumServiceTypes(3, "Handlers");
        public static EnumServiceTypes Edits = new EnumServiceTypes(4, "Edits");
        public static EnumServiceTypes Filters = new EnumServiceTypes(5, "Filters");
        public static EnumServiceTypes Rules = new EnumServiceTypes(6, "Rules");

        protected EnumServiceTypes(){
        }

        public EnumServiceTypes(int id, string name)
            : base(id, name){
        }

        public static IEnumerable<EnumServiceTypes> List()
        {
            return new[] { None,Common, Handlers, Edits, Filters, Rules };
        }
    }
}
