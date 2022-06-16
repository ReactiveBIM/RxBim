namespace RxBim.Application.Ribbon.Services.AddItemStrategies
{
    using Autodesk.Revit.UI;

    /// <summary>
    /// Implementation of <see cref="IAddItemStrategy"/> for command button.
    /// </summary>
    public class CommandButtonStrategy : ItemStrategyBase<CommandButton>
    {
        /// <inheritdoc />
        public CommandButtonStrategy(MenuData menuData)
            : base(menuData)
        {
        }

        /// <inheritdoc />
        protected override void CreateAndAddItem(RibbonPanel panel, CommandButton cmdButtonConfig)
        {
            var pushButtonData = CreateCommandButtonData(cmdButtonConfig);
            panel.AddItem(pushButtonData);
        }

        /// <inheritdoc />
        protected override RibbonItemData CreateItemForStack(CommandButton cmdButtonConfig)
        {
            return CreateCommandButtonData(cmdButtonConfig);
        }
    }
}