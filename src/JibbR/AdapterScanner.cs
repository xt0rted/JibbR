﻿using System;
using System.Linq;

using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace JibbR
{
    public class AdapterScanner : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            var interfaces = type.GetInterfaces();
            if (!interfaces.Contains(typeof (IRobotAdapter)))
            {
                return;
            }

            var attributes = type.GetCustomAttributes(typeof (RobotAdapterMetadataAttribute), false);
            if (attributes.Length < 1)
            {
                Console.WriteLine("Type '{0}' is missing metadata, will be skipped.", type.Name);
                return;
            }

            var attribute = (RobotAdapterMetadataAttribute) attributes[0];
            var name = attribute.GetAdapterName();

            registry.AddType(typeof (IRobotAdapter), type, name);
        }
    }
}