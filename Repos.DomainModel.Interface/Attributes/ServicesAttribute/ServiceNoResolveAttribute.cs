using System;

namespace Repos.DomainModel.Interface.Atrributes.ServiceAttributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class ServiceNoResolveAttribute : Attribute
    {

    }
}
