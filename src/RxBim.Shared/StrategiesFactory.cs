namespace RxBim.Shared
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Abstractions;
    using Di;

    /// <inheritdoc />
    public class StrategiesFactory<T> : IStrategyFactory<T>
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
        public void AddStrategies(Assembly assembly)
        {
            var types = assembly.GetTypes()
                .Where(x => x.IsAssignableFrom(typeof(T)) && !x.IsAbstract && !x.IsInterface)
                .Except(_container.GetCurrentRegistrations().Select(x => x.ServiceType))
                .ToList();

            foreach (var type in types)
            {
                _container.AddTransient(type);
            }
        }

        /// <inheritdoc />
        public IEnumerable<T> GetStrategies()
        {
            var strategyInterfaceType = typeof(T);
            return _container.GetCurrentRegistrations()
                .Where(x => x.ServiceType.IsAssignableFrom(strategyInterfaceType))
                .Select(x => _container.GetService(x.ServiceType))
                .Cast<T>();
        }
    }
}