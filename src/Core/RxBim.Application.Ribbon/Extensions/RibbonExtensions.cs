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
        public static IPanelBuilder CommandButton<TCommand>(
            this IPanelBuilder parent,
            string name,
            Action<IButtonBuilder>? builder = null)
        {
            var commandType = typeof(TCommand);
            var attr = commandType.GetCustomAttribute<RxBimCommandAttribute>(true);
            return parent.CommandButton(
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
        public static IPulldownButtonBuilder CommandButton<TCommand>(
            this IPulldownButtonBuilder parent,
            string name,
            Action<IButtonBuilder>? builder = null)
        {
            var commandType = typeof(TCommand);
            var attr = commandType.GetCustomAttribute<RxBimCommandAttribute>(true);
            return parent.CommandButton(
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
        public static IStackedItemsBuilder CommandButton<TCommand>(
            this IStackedItemsBuilder parent,
            string name,
            Action<IButtonBuilder>? builder = null)
        {
            var commandType = typeof(TCommand);
            var attr = commandType.GetCustomAttribute<RxBimCommandAttribute>(true);
            return parent.CommandButton(
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
                .Description(attr.Description!)
                .Text(attr.Text!)
                .ToolTip(attr.ToolTip!)
                .HelpUrl(attr.HelpUrl!)
                .SmallImage(attr.SmallImage!, ThemeType.Dark)
                .LargeImage(attr.LargeImage!, ThemeType.Dark)
                .SmallImage(attr.SmallImageLight!, ThemeType.Light)
                .LargeImage(attr.LargeImageLight!, ThemeType.Light);
        }
    }
}