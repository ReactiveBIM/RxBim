#pragma warning disable SA1600
namespace RxBim.Logs.Settings.Configuration
{
    using System;

    internal interface IConfigurationArgumentValue
    {
        object? ConvertTo(Type toType, ResolutionContext resolutionContext);
    }
}
