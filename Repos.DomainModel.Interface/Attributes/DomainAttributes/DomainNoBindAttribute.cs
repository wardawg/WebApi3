using System;

namespace Repos.DomainModel.Interface.Atrributes.DomainAttributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class DomainNoBindAttribute : Attribute
    {

    }
}
