namespace RxBim.Application.Ribbon.Revit.Services
{
    using System;
    using Abstractions;
    using Abstractions.ConfigurationBuilders;
    using Ribbon.Services;

    /// <inheritdoc />
    public class RevitMenuBuildService : MenuBuildServiceBase
    {
        /// <inheritdoc />
        public RevitMenuBuildService(IRibbonFactory ribbonFactory, Action<IRibbonBuilder> action)
            : base(ribbonFactory, action)
        {
        }
    }
}