using Repos.DomainModel.Interface.Attributes.DynamicAttributes;
using Repos.DomainModel.Interface.DomainComplexTypes;
using Repos.DomainModel.Interface.Filters;
using System;
using System.Collections.Generic;
using System.Dynamic;
using Repos.DomainModel.Interface.Interfaces.Filter;
using System.Linq;

namespace Repos.DomainModel.Interface.Atrributes.DynamicAttributes
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class FilterKeyPair
          : AttributeList
          , IFilterKeyPair
    {
        public FilterKeyPair()
        {
            _attributeType = EnumAttributes.filter;
        }
        public string filterValue { set; get; }
        public List<string> filterValues { get; set; } = new List<string>();
        private dynamic _refFilter;
        
        public IEnumerable<IFilterKeyPair> GetFilterValues(DomainEntityType source)
        {
            SetValue(source);
            return source.Attributes.Filter.Filters;
        }


        public void SetValue(DomainEntityType source
                            , IDomainActionFilter filter
                           , IEnumerable<IFilterKeyPair> filters)
        {
            if (_refFilter == null)
                _refFilter = filter;

            _refFilter.SetAttribute(source, filters);
        }

        public void SetValue(DomainEntityType source)
        {
            if (source.HasAttributes(EnumAttributes.filter))
                return;

            _refFilter.SetAttribute(source);
                                

        }

        public IEnumerable<IFilterKeyPair> GetFilterValues()
        {
            return _refFilter.GetFilterAttribute().Filters;
        }
                      

        public void SetRef(IFilter refFilter)
        {
            _refFilter = refFilter;
        }
       
    }
}
