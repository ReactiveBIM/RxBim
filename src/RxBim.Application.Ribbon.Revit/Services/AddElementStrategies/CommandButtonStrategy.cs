namespace RxBim.Application.Ribbon.Revit.Services.AddElementStrategies
{
    using Abstractions;
    using Autodesk.Revit.UI;
    using Models;
    using Models.Configurations;

    /// <summary>
    /// Implementation of <see cref="IAddElementStrategy"/> for command button.
    /// </summary>
    public class CommandButtonStrategy : ElementStrategyBase<CommandButton>
    {
        /// <inheritdoc />
        public CommandButtonStrategy(MenuData menuData)
            : base(menuData)
        {
        }

        /// <inheritdoc />
        protected override void CreateAndAddElement(string tabName, RibbonPanel panel, CommandButton cmdButtonConfig)
        {
            var pushButtonData = CreateCommandButtonData(cmdButtonConfig);
            panel.AddItem(pushButtonData);
        }

        /// <inheritdoc />
        protected override RibbonItemData CreateElementForStack(CommandButton cmdButtonConfig)
        {
            return CreateCommandButtonData(cmdButtonConfig);
        }
    }
}