// These substitutes must never be used to execute a command's host-dependent DI path.
namespace RxBim.Di
{
    using System;
    using System.Reflection;

    internal sealed class CommandDiConfigurator
    {
        public CommandDiConfigurator(object command) =>
            throw new InvalidOperationException("Unexpected execution in the original context.");

        public void Configure(Assembly assembly) => throw new NotSupportedException();

        public IServiceProvider Build() => throw new NotSupportedException();
    }
}

namespace Microsoft.Extensions.DependencyInjection
{
    using System;

    internal static class ServiceProviderServiceExtensions
    {
        public static T GetRequiredService<T>(this IServiceProvider provider) =>
            throw new NotSupportedException();
    }
}

namespace Autodesk.AutoCAD.ApplicationServices.Core
{
    internal static class Application
    {
        public static string LastAlert { get; private set; } = string.Empty;

        public static void ShowAlertDialog(string message) => LastAlert = message;
    }
}