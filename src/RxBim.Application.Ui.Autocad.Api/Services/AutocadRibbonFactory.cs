namespace RxBim.Application.Ui.Autocad.Api.Services
{
    using Abstractions;
    using Di;
    using Models;
    using RxBim.Application.Ui.Api.Abstractions;

    /// <inheritdoc />
    public class AutocadRibbonFactory : IRibbonFactory
    {
        private readonly IOnlineHelpService _onlineHelpService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutocadRibbonFactory"/> class.
        /// </summary>
        /// <param name="onlineHelpService">Сервис онлайн-справки</param>
        public AutocadRibbonFactory(IOnlineHelpService onlineHelpService)
        {
            _onlineHelpService = onlineHelpService;
        }

        /// <inheritdoc />
        public IRibbon Create(IContainer container)
        {
            _onlineHelpService.Run();
            return new Ribbon(container);
        }
    }
}