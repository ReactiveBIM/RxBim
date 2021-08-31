namespace RxBim.Di
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