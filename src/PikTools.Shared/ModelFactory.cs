namespace PikTools.Shared
{
    using System;
    using PikTools.Shared.Abstractions;
    using SimpleInjector;

    /// <inheritdoc/>
    public class ModelFactory : IModelFactory
    {
        private readonly Container _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelFactory"/> class.
        /// </summary>
        /// <param name="container">Конфигурация приложения</param>
        public ModelFactory(Container container)
        {
            _container = container;
        }

        /// <inheritdoc />
        public T GetInstance<T>()
            where T : class
            => _container.GetInstance<T>();

        /// <inheritdoc />
        public object GetInstance(Type type)
            => _container.GetInstance(type);
    }
}
