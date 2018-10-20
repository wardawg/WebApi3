using Repos.DomainModel.Interface.DomainComplexTypes;
using System;

namespace Repos.DomainModel.Interface.DomainComplexTypes
{
    public static class DomainEntityFactory
    {
       public static DomainEntityType GetDomainEntityType(string EntityType)
        {

            var entityType = default(DomainEntityType);

            switch (EntityType.ToLower())
            {

                case "domainentitytypedecimal":
                    entityType = new DomainEntityTypeDecimal();
                    break;
                case "domainentitytypedatetime":
                    entityType = new DomainEntityTypeDateTime();
                    break;
                case "domainentitytypestring":
                    entityType = new DomainEntityTypeString();
                    break;
                case "domainentitytypedouble":
                    entityType = new DomainEntityTypeDouble();
                    break;
                case "domainentitytypeint":
                case "domainentitytypeint32":
                    entityType = new DomainEntityTypeInt();
                    break;
                default:
                    throw new Exception("Cannot Determine Factory Type For: " + EntityType.ToLower());
            }
            return entityType;
        }
    }
}
