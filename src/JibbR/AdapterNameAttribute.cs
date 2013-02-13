using System;

namespace JibbR
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class AdapterNameAttribute : Attribute
    {
        public AdapterNameAttribute(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            Name = name.Trim();
        }

        public string Name { get; private set; }
    }
}