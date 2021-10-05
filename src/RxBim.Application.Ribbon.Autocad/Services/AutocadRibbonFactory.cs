namespace RxBim.Application.Ribbon.Autocad.Services
{
    using Di;
    using Models;
    using Ribbon.Abstractions;

    /// <inheritdoc />
    public class AutocadRibbonFactory : IRibbonFactory
    {
        /// <inheritdoc />
        public IRibbon Create(IContainer container)
        {
            return new Ribbon(container);
        }
    }
}