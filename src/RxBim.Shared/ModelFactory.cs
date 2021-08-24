namespace RxBim.Shared
{
    using System;
    using Abstractions;
    using Di;

    /// <inheritdoc/>
    public class ModelFactory : IModelFactory
    {
        private readonly IContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelFactory"/> class.
        /// </summary>
        /// <param name="container">Конфигурация приложения</param>
        public ModelFactory(IContainer container)
        {
            _container = container;
        }

        /// <inheritdoc />
        public T GetInstance<T>()
            where T : class
            => _container.GetService<T>();

        /// <inheritdoc />
        public object GetInstance(Type type)
            => _container.GetService(type);
    }
}
