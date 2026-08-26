namespace RxBim.Di.Tests;

using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Options;
using Xunit;

public class Net8DependencyTests
{
    private const int ExpectedMajorVersion = 8;

    [Fact]
    public void MicrosoftDependenciesShouldUseNet8Versions()
    {
        var assemblies = new[]
        {
            typeof(IConfiguration).Assembly,
            typeof(ConfigurationBuilder).Assembly,
            typeof(BinderOptions).Assembly,
            typeof(JsonConfigurationSource).Assembly,
            typeof(ServiceCollection).Assembly,
            typeof(DependencyContext).Assembly,
            typeof(Options).Assembly,
            typeof(OptionsConfigurationServiceCollectionExtensions).Assembly,
        };
        var majorVersions = assemblies.Select(assembly => assembly.GetName().Version?.Major);

        majorVersions.Should().OnlyContain(majorVersion => majorVersion == ExpectedMajorVersion);
    }
}