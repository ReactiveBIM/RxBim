#pragma warning disable

namespace RxBim.Logs.Settings.Configuration
{
    using System;

    interface IConfigurationArgumentValue
    {
        object ConvertTo(Type toType, ResolutionContext resolutionContext);
    }
}
