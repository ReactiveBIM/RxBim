namespace RxBim.Application.Ribbon.Abstractions
{
    using System;
    using Di;

    /// <summary>
    /// Панель
    /// </summary>
    public interface IPanel : IRibbonBuilder, IRibbonElement
    {
        /// <summary>
        /// The tab on which the panel is located
        /// </summary>
        ITab Tab { get; }

        /// <summary>
        /// Create new Stacked items at the panel
        /// </summary>
        /// <param name="action">Action where you must add items to the stacked panel</param>
        /// <returns>Panel where stacked items were created</returns>
        IPanel StackedItems(Action<IStackedItem> action);

        /// <summary>
        /// Create push button on the panel
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <param name="externalCommandType">Class which implements IExternalCommand interface.
        /// This command will be execute when user push the button</param>
        /// <param name="action">Additional action with whe button</param>
        /// <returns>Panel where button were created</returns>
        IPanel Button(string name, string text, Type externalCommandType, Action<IButton> action = null);

        /// <summary>
        /// Create pull down button on the panel
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <param name="action">Additional action with whe button</param>
        /// <returns>Panel where button were created</returns>
        IPanel PullDownButton(string name, string text, Action<IPulldownButton> action);

        /// <summary>
        /// Create separator on the panel
        /// </summary>
        /// <returns></returns>
        IPanel Separator();

        /// <summary>
        /// Добавляет на панель кнопку "О программе"
        /// </summary>
        /// <param name="name">Название</param>
        /// <param name="text">Описание</param>
        /// <param name="tabName">Название вкладки плагина</param>
        /// <param name="panelName">Название панели</param>
        /// <param name="container">Контейнер</param>
        /// <param name="action">Действие</param>
        void AddAboutButton(
            string name,
            string text,
            string tabName,
            string panelName,
            IContainer container,
            Action<IAboutButton> action);
    }
}