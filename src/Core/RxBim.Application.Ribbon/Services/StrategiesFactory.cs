namespace RxBim.Application.Ribbon
{
    using System.Collections.Generic;
    using System.Linq;
    using Di;

    /// <inheritdoc />
    internal class StrategiesFactory<T> : IStrategiesFactory<T>
    {
        private readonly IContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="StrategiesFactory{T}"/> class.
        /// </summary>
        /// <param name="container">DI container.</param>
        public StrategiesFactory(IContainer container)
        {
            _container = container;
        }

        /// <inheritdoc />
        public IEnumerable<T> GetStrategies()
        {
            var interfaceType = typeof(T);
            return _container.GetCurrentRegistrations()
                .Where(x => interfaceType.IsAssignableFrom(x.ServiceType))
                .Select(x => _container.GetService(x.ServiceType))
                .Cast<T>();
        }
    }
}