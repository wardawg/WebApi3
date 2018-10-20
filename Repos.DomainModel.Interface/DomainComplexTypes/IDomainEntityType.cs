using System.Collections.Generic;

namespace Repos.DomainModel.Interface.DomainComplexTypes
{


    public interface IDomainEntityType
    {
        dynamic Attributes { set; get; }
        bool Enabled { set; get; }

        void SetClean();
    }

    public interface IDomainEntityType<T> 
        : IDomainEntityType
    {
        T Value { set; get; }
    }
}
