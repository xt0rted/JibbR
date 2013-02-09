﻿using System;
using System.Collections.Generic;
using System.Linq;

using StructureMap;

namespace JibbR
{
    public class AdapterManager : IAdapterManager
    {
        private readonly IContainer _container;

        private bool _adaptersLoaded;

        public AdapterManager(IContainer container)
        {
            _container = container;

            Adapters = new List<IRobotAdapter>();
        }

        public IList<IRobotAdapter> Adapters { get; private set; }

        public void SetupAdapters(IRobot robot)
        {
            if (_adaptersLoaded)
            {
                return;
            }

            _adaptersLoaded = true;

            foreach (var adapter in robot.Settings.Adapters.Where(a => a.Enabled).Select(a => a.Name))
            {
                Console.WriteLine("Trying to load adapter named '{0}'", adapter);

                var instance = _container.TryGetInstance<IRobotAdapter>(adapter);
                if (instance == null)
                {
                    Console.WriteLine("No adapter found named '{0}'", adapter);
                    continue;
                }

                instance.Setup(robot);
                Adapters.Add(instance);
            }
        }
    }
}