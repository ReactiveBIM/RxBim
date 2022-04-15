namespace RxBim.Di
{
    using System;

    /// <summary>
    /// Represents a service registration record in a DI container.
    /// </summary>
    public class Registration
    {
        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="serviceType">A type of a registered service.</param>
        public Registration(Type serviceType)
        {
            ServiceType = serviceType;
        }

        /// <summary>
        /// The registered service type.
        /// </summary>
        public Type ServiceType { get; }
    }
}