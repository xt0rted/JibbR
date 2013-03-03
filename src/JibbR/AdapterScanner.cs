using System;

using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace JibbR
{
    public class AdapterScanner : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (!typeof (IRobotAdapter).IsAssignableFrom(type))
            {
                return;
            }

            var attributes = type.GetCustomAttributes(typeof (AdapterNameAttribute), false);
            if (attributes.Length < 1)
            {
                Console.WriteLine("Type '{0}' is missing metadata, will be skipped.", type.Name);
                return;
            }

            var attribute = (AdapterNameAttribute) attributes[0];
            var name = attribute.Name;

            registry.AddType(typeof (IRobotAdapter), type, name);
        }
    }
}