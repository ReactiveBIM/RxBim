namespace RxBim.Application.Ribbon.Revit.Services
{
    using System;
    using Abstractions;
    using Ribbon.Services;

    /// <inheritdoc />
    public class RevitMenuBuildService : MenuBuildServiceBase
    {
        /// <inheritdoc />
        public RevitMenuBuildService(IRibbonFactory ribbonFactory, Action<IRibbon> action)
            : base(ribbonFactory, action)
        {
        }
    }
}