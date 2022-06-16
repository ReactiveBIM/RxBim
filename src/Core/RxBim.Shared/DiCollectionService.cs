namespace RxBim.Shared
{
    using System.Collections.Generic;
    using System.Linq;
    using Abstractions;
    using Di;

    /// <inheritdoc />
    internal class DiCollectionService<T> : IDiCollectionService<T>
    {
        private readonly IContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiCollectionService{T}"/> class.
        /// </summary>
        /// <param name="container">DI container.</param>
        public DiCollectionService(IContainer container)
        {
            _container = container;
        }

        /// <inheritdoc />
        public IEnumerable<T> GetItems()
        {
            var interfaceType = typeof(T);
            return _container.GetCurrentRegistrations()
                .Where(x => interfaceType.IsAssignableFrom(x.ServiceType))
                .Select(x => _container.GetService(x.ServiceType))
                .Cast<T>();
        }
    }
}