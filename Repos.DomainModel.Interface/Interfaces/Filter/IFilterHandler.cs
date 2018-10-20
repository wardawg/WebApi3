using System.Collections.Generic;

namespace Repos.DomainModel.Interface.Interfaces.Filter
{
    public interface IFilterHandler : IFilterEnum
    {
        dynamic FilterSource { get; set; }
        Dictionary<string, object> ParmInputs { get; set; }

    }
}
