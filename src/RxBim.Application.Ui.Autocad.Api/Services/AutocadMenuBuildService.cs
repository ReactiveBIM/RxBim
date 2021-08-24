namespace RxBim.Application.Ui.Autocad.Api.Services
{
    using System;
    using Abstractions;
    using Di;
    using RxBim.Application.Ui.Api.Abstractions;
    using RxBim.Application.Ui.Api.Services;

    /// <inheritdoc />
    public class AutocadMenuBuildService : MenuBuildServiceBase
    {
        private readonly IRibbonEvents _ribbonEvents;
        private readonly IOnlineHelpService _onlineHelpService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuBuildServiceBase"/> class.
        /// </summary>
        /// <param name="ribbonFactory">Фабрика ленты</param>
        /// <param name="action">Действие по построению ленты</param>
        /// <param name="ribbonEvents">Сервис событий ленты</param>
        /// <param name="onlineHelpService">Сервис онлайн-справки</param>
        public AutocadMenuBuildService(
            IRibbonFactory ribbonFactory,
            Action<IRibbon> action,
            IRibbonEvents ribbonEvents,
            IOnlineHelpService onlineHelpService)
            : base(ribbonFactory, action)
        {
            _ribbonEvents = ribbonEvents;
            _onlineHelpService = onlineHelpService;
        }

        /// <inheritdoc />
        public override void BuildMenu(IContainer container)
        {
            base.BuildMenu(container);

            _onlineHelpService.Run();

            _ribbonEvents.Run();
            _ribbonEvents.NeedRebuild += (_, _) => BuildMenuInternal();
        }
    }
}