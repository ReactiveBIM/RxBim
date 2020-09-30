#pragma warning disable

namespace PikTools.Logs.Settings.Configuration
{
    using System;

    interface IConfigurationArgumentValue
    {
        object ConvertTo(Type toType, ResolutionContext resolutionContext);
    }
}
