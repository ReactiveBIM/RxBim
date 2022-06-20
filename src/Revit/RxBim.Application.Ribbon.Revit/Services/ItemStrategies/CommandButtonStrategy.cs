namespace RxBim.Application.Ribbon.Services.ItemStrategies
{
    using Autodesk.Revit.UI;

    /// <summary>
    /// Implementation of <see cref="IItemStrategy"/> for command button.
    /// </summary>
    public class CommandButtonStrategy : ItemStrategyBase<CommandButton>
    {
        private readonly MenuData _menuData;

        /// <inheritdoc />
        public CommandButtonStrategy(MenuData menuData)
            : base(menuData)
        {
            _menuData = menuData;
        }

        /// <inheritdoc />
        protected override void AddItem(string tabName, RibbonPanel panel, CommandButton cmdButtonConfig)
        {
            cmdButtonConfig.LoadFromAttribute(_menuData.MenuAssembly);
            var pushButtonData = CreateCommandButtonData(cmdButtonConfig);
            panel.AddItem(pushButtonData);
        }

        /// <inheritdoc />
        protected override RibbonItemData GetItemForStack(CommandButton cmdButtonConfig)
        {
            cmdButtonConfig.LoadFromAttribute(_menuData.MenuAssembly);
            return CreateCommandButtonData(cmdButtonConfig);
        }
    }
}