using Repos.DomainModel.Interface.Attributes.DynamicAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;

namespace Repos.DomainModel.Interface.DomainComplexTypes
{

    public class DomainEntityType
    {
        protected dynamic theAttribute = new ExpandoObject();

        [NotMapped]
        public dynamic Attributes
        {
            get
            {
                return theAttribute;
            }

            set
            {
                if (value == null)
                    value = new ExpandoObject();
                
                theAttribute = value;
            }

        }
        public bool HasAttributes(EnumAttributes enumAttrb)
        {
            Boolean? bAttrib = false;

            try
            {

                        bAttrib = ((IDictionary<string, object>)theAttribute)?
                        .Any(a => a.Key.ToLower() == enumAttrb.ToString().ToLower());
            }
            catch
            {

            }

            return bAttrib.HasValue ? bAttrib.Value : false;

        }

    }

   public abstract class DomainEntityType<T> 
        : DomainEntityType 
        , IDomainEntityType<T>  
        
    {
               
        private T theValue = default(T);
        public  T Value {
                        set {
                                if (IsNull || 
                                    (value != null && !value.Equals(theValue)))
                                {
                                    IsDirty = true;
                                    theValue = value;
                                    IsNull = false;
                                }

                            }
                         get { return theValue; }
                        }

        

        
        
        
        [NotMapped]
        public bool IsDirty { get; private set; } = false;
        [NotMapped]
        public bool IsNull { get; private set; } = true;
        [NotMapped]
        public bool Enabled { get; set; } = true;

        void IDomainEntityType.SetClean()
        {
            IsDirty = false;
        }
    }
}
