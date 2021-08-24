namespace RxBim.Application.Ui.Revit.Api.Services
{
    using System;
    using RxBim.Application.Ui.Api.Abstractions;
    using RxBim.Application.Ui.Api.Services;

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