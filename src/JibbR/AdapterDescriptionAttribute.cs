using System;

namespace JibbR
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class AdapterDescriptionAttribute : Attribute
    {
        public AdapterDescriptionAttribute(string name)
        {
            Name = name;
        }

        public AdapterDescriptionAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; private set; }
        public string Description { get; set; }
    }
}