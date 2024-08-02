namespace RxBim.Di;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// DI container.
/// </summary>
public class DiContainer : IContainer
{
    private readonly IServiceCollection _services = null!;
    private IServiceProvider? _provider;
    private EventHandler? _containerBuilt;

    /// <summary>
    /// Initializes a new instance of the <see cref="DiContainer"/> class.
    /// </summary>
    public DiContainer()
    {
        _services = new ServiceCollection();
    }

    /// <summary>
    /// Internal ctor.
    /// </summary>
    /// <param name="serviceProvider">The scoped service provider.</param>
    private DiContainer(IServiceProvider serviceProvider)
    {
        _provider = serviceProvider;
    }

    /// <inheritdoc />

    public event EventHandler? ContainerBuilt
    {
        add
        {
            EnsureContainerNotBuilt();
            _containerBuilt += value;
        }
        remove => _containerBuilt -= value;
    }

    /// <inheritdoc />
    public IServiceCollection Services
    {
        get
        {
            EnsureContainerNotBuilt();
            return _services;
        }
    }

    /// <inheritdoc />
    public IServiceProvider ServiceProvider
    {
        get
        {
            if (_provider is null)
            {
                _provider = Services.BuildServiceProvider(false);
                _containerBuilt?.Invoke(this, EventArgs.Empty);
                _containerBuilt = null;
            }

            return _provider;
        }
    }

    /// <inheritdoc />
    public IContainer Add(Type serviceType, Type implementationType, Lifetime lifetime)
    {
        Add(Services, serviceType, implementationType, lifetime);
        return this;
    }

    /// <inheritdoc />
    public IContainer Add(Type serviceType, Func<object> implementationFactory, Lifetime lifetime)
    {
        Add(Services, serviceType, _ => implementationFactory(), lifetime);
        return this;
    }

    /// <inheritdoc />
    public IContainer AddSingleton(Type serviceType, object implementationInstance)
    {
        Services.AddSingleton(serviceType, implementationInstance);
        return this;
    }

    /// <inheritdoc />
    public IContainer AddInstance(Type serviceType, object implementationInstance)
    {
        Services.AddTransient(serviceType, _ => implementationInstance);
        return this;
    }

    /// <inheritdoc />
    public IContainer Decorate(Type serviceType, Type decoratorType)
    {
        Services.Decorate(serviceType, decoratorType);
        return this;
    }

    /// <inheritdoc />
    public IEnumerable<Registration> GetCurrentRegistrations()
    {
        return _services.Select(x => new Registration(x.ServiceType));
    }

    /// <inheritdoc />
    public object GetService(Type serviceType) => ServiceProvider.GetService(serviceType);

    /// <inheritdoc />
    public IContainerScope CreateScope()
    {
        var scope = ServiceProvider.CreateScope();
        var container = new DiContainer(scope.ServiceProvider);
        return new ContainerScope(scope, container);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_provider is IDisposable disposable)
            disposable.Dispose();
    }

    private static void Add(
        IServiceCollection collection,
        Type serviceType,
        Type implementationType,
        Lifetime lifetime)
    {
        var descriptor = new ServiceDescriptor(serviceType, implementationType, GetServiceLifetime(lifetime));
        collection.Add(descriptor);
    }

    private static void Add(
        IServiceCollection collection,
        Type serviceType,
        Func<IServiceProvider, object> implementationFactory,
        Lifetime lifetime)
    {
        var descriptor = new ServiceDescriptor(serviceType, implementationFactory, GetServiceLifetime(lifetime));
        collection.Add(descriptor);
    }

    private static ServiceLifetime GetServiceLifetime(Lifetime lifetime) => lifetime switch
    {
        Lifetime.Transient => ServiceLifetime.Transient,
        Lifetime.Scoped => ServiceLifetime.Scoped,
        Lifetime.Singleton => ServiceLifetime.Singleton,
        _ => throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null)
    };

    private void EnsureContainerNotBuilt()
    {
        if (_provider != null)
            throw new InvalidOperationException("Container is already built");
    }
}