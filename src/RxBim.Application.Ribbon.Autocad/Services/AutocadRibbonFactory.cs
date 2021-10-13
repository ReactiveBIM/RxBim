namespace RxBim.Application.Ribbon.Autocad.Services
{
    using Di;
    using Models;
    using Ribbon.Abstractions;
    using Ribbon.Services;

    /// <inheritdoc />
    public class AutocadRibbonFactory : RibbonFactoryBase
    {
        /// <inheritdoc />
        protected override IRibbonBuilder Create(IContainer container)
        {
            return new RibbonBuilder(container);
        }
    }
}