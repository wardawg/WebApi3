using DependencyResolver;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace ProjectDependResolver
{

    public enum enumConfigOpts
    {
         None
       , ForceNameResolve
       , NoBaseImplementation
       , NoDLLValidation
       , RegAll
       
    }
    public abstract class ConfOptions
           : IConfigOptions
    {
        private enumConfigOpts _options;

        public abstract enumConfigOpts ConfigOptions { get; } //=> new enumTest(); }

        //dynamic IConfigOptions.ConfigOptions => throw new NotImplementedException();

        public void Add(dynamic option)
        {
            _options = option; //  .Add(option);
        }

        public abstract bool Remove(enumConfigOpts EnuOption);
        
        public abstract void Add(enumConfigOpts EnuOption);

        public bool Contains(dynamic option)
        {

            bool vb = false;

            List<string> nets_List = new List<string>() { "A", "B", "C" };
            var netListEnumType = GenerateEnumerations(nets_List, "netsenum");


            var c = (enumConfigOpts)option;

            if (_options == c)
                vb = true;

                return true;
        }

        public abstract bool Contains(enumConfigOpts option);

        

        private Type GenerateEnumerations(List<string> lEnumItems, string assemblyName)
        {
            //    Create Base Assembly Objects
            AppDomain appDomain = AppDomain.CurrentDomain;
            AssemblyName asmName = new AssemblyName(assemblyName);
            AssemblyBuilder asmBuilder = appDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);

            //    Create Module and Enumeration Builder Objects
            ModuleBuilder modBuilder = asmBuilder.DefineDynamicModule(assemblyName + "_module");
            EnumBuilder enumBuilder = modBuilder.DefineEnum(assemblyName, TypeAttributes.Public, typeof(int));
            enumBuilder.DefineLiteral("None", 0);
            int flagCnt = 1;
            foreach (string fmtObj in lEnumItems)
            {
                enumBuilder.DefineLiteral(fmtObj, flagCnt);
                flagCnt++;
            }
            var retEnumType = enumBuilder.CreateType();
            //asmBuilder.Save(asmName.Name + ".dll");
            return retEnumType;
        }
    }
}
