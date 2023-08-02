namespace RxBim.Di;

using System;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// DI container.
/// </summary>
public class DiContainer : IContainer
{
    private readonly IServiceCollection _services;
    private readonly Lazy<IServiceProvider> _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="DiContainer"/> class.
    /// </summary>
    public DiContainer()
    {
        _services = new ServiceCollection();
        _provider = new Lazy<IServiceProvider>(() => Services.BuildServiceProvider(false));
    }

    /// <inheritdoc />
    public IServiceCollection Services
    {
        get
        {
            if (_provider.IsValueCreated)
                throw new InvalidOperationException("Container is already built");

            return _services;
        }
    }

    /// <inheritdoc />
    public IServiceProvider ServiceProvider => _provider.Value;
}