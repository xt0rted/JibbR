using System;

namespace JibbR
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    internal sealed class RobotAdapterMetadataAttribute : Attribute
    {
        private readonly string _adapterName;

        public RobotAdapterMetadataAttribute(string adapterName)
        {
            if (string.IsNullOrWhiteSpace(adapterName))
            {
                throw new ArgumentNullException("adapterName");
            }

            _adapterName = adapterName.Trim();
        }

        public string GetAdapterName()
        {
            return _adapterName;
        }
    }
}