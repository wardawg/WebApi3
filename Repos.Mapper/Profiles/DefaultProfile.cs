
using System;
using System.Collections.Generic;
using Repos.Mapper.Entities;

namespace Repos.Mapper.Profiles
{
    internal class DefaultProfile<IComplexType, ITarget> 
        : MapperProfile<IComplexType, ITarget>
    {

         protected override IEnumerable<MapTarget> SetMaps(){
            return new List<MapTarget>();
        }
        

        public override string ProfileName
        {
            get
            {
                return "Default";
            }
        }
    }
    
    
}