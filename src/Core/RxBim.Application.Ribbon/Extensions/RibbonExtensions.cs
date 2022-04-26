namespace RxBim.Application.Ribbon
{
    using System;
    using System.Reflection;
    using Shared;

    /// <summary>
    /// Contains ribbon builder methods.
    /// </summary>
    public static class RibbonExtensions
    {
        /// <summary>
        /// Adds a new push button to the panel.
        /// </summary>
        /// <param name="parent">A pannel builder.</param>
        /// <param name="name">Internal name of the button.</param>
        /// <param name="builder">The button builder.</param>
        /// <typeparam name="TCommand">TClass which implements IExternalCommand interface.
        /// This command will be execute when user push the button.</typeparam>
        public static IPanelBuilder AddCommandButton<TCommand>(
            this IPanelBuilder parent,
            string name,
            Action<IButtonBuilder>? builder = null)
        {
            var commandType = typeof(TCommand);
            var attr = commandType.GetCustomAttribute<RxBimCommandAttribute>(true);
            return parent.AddCommandButton(
                name,
                typeof(TCommand),
                x =>
                {
                    AttributeAction(x, attr);
                    builder?.Invoke(x);
                });
        }

        /// <summary>
        /// Adds a new push button to the panel.
        /// </summary>
        /// <param name="parent">A pannel builder.</param>
        /// <param name="name">Internal name of the button.</param>
        /// <param name="builder">The button builder.</param>
        /// <typeparam name="TCommand">TClass which implements IExternalCommand interface.
        /// This command will be execute when user push the button.</typeparam>
        public static IPulldownButtonBuilder AddCommandButton<TCommand>(
            this IPulldownButtonBuilder parent,
            string name,
            Action<IButtonBuilder>? builder = null)
        {
            var commandType = typeof(TCommand);
            var attr = commandType.GetCustomAttribute<RxBimCommandAttribute>(true);
            return parent.AddCommandButton(
                name,
                typeof(TCommand),
                x =>
                {
                    AttributeAction(x, attr);
                    builder?.Invoke(x);
                });
        }

        /// <summary>
        /// Adds a new push button to the panel.
        /// </summary>
        /// <param name="parent">A pannel builder.</param>
        /// <param name="name">Internal name of the button.</param>
        /// <param name="builder">The button builder.</param>
        /// <typeparam name="TCommand">TClass which implements IExternalCommand interface.
        /// This command will be execute when user push the button.</typeparam>
        public static IStackedItemsBuilder AddCommandButton<TCommand>(
            this IStackedItemsBuilder parent,
            string name,
            Action<IButtonBuilder>? builder = null)
        {
            var commandType = typeof(TCommand);
            var attr = commandType.GetCustomAttribute<RxBimCommandAttribute>(true);
            return parent.AddCommandButton(
                name,
                typeof(TCommand),
                x =>
                {
                    AttributeAction(x, attr);
                    builder?.Invoke(x);
                });
        }

        private static IButtonBuilder AttributeAction(IButtonBuilder button, RxBimCommandAttribute? attr)
        {
            if (attr == null)
                return button;

            return button
                .SetDescription(attr.Description!)
                .SetText(attr.Text!)
                .SetToolTip(attr.ToolTip!)
                .SetHelpUrl(attr.HelpUrl!)
                .SetSmallImage(attr.SmallImage!, ThemeType.Dark)
                .SetLargeImage(attr.LargeImage!, ThemeType.Dark)
                .SetSmallImage(attr.SmallImageLight!, ThemeType.Light)
                .SetLargeImage(attr.LargeImageLight!, ThemeType.Light);
        }
    }
}