using Repos.DomainModel.Interface.Attributes.DynamicAttributes;

namespace Repos.DomainModel.Interface.Atrributes.DynamicAttributes
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AttributeList
    {
        protected EnumAttributes _attributeType = EnumAttributes.none;

        protected EnumAttributes AttributeType { get { return _attributeType; } }
    }
}
