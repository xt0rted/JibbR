using System;

namespace JibbR
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class AdapterUsageAttribute : Attribute
    {
        public AdapterUsageAttribute(string name, string example)
        {
            Name = name;
            Example = example;
        }

        public string Name { get; private set; }
        public string Example { get; set; }
    }
}