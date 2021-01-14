#pragma warning disable
namespace PikTools.Di
{
    using System;

    public class Registration
    {
        public Registration(Type serviceType)
        {
            ServiceType = serviceType;
        }

        public Type ServiceType { get; }
    }
}