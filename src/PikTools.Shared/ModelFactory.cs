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
        {
            var type = typeof(T);
            var instance = _container.GetInstance(type);

            return (T)instance;
        }

        /// <inheritdoc />
        public object GetInstance(Type type)
        {
            return _container.GetInstance(type);
        }
    }
}
