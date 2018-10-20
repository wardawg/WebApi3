using System;
using System.Configuration;
using System.Xml;
using ReposCore.Configuration;

namespace ReposServiceConfigurations
{

    /// <summary>
    /// ServiceConfig
    /// Configuration 
    /// For Repos Apps
    /// </summary>
    public class ServiceConfig : IConfigurationSectionHandler

    {
        public string ContextName { get; private set; }
        public string ResolverType { get; private set; }
        public string RuntimePrefixes { get; private set; }
        public bool DLLValidation { get; private set; }
        public object Create(object parent, object configContext, XmlNode section)
        {

            var config = new ServiceConfig();
            var DBNode = section.SelectSingleNode("DLLValidation");
            config.DLLValidation = Convert.ToBoolean(GetString(DBNode, "ValidateDLL"));
            return config;

        }

        private T SetByXElement<T>(XmlNode node, string attrName, Func<string, T> converter)
        {
            if (node == null || node.Attributes == null) return default(T);
            var attr = node.Attributes[attrName];
            if (attr == null) return default(T);
            var attrVal = attr.Value;
            return converter(attrVal);
        }


        private string GetString(XmlNode node, string attrName)
        {
            return SetByXElement<string>(node, attrName, Convert.ToString);
        }
    }
}
