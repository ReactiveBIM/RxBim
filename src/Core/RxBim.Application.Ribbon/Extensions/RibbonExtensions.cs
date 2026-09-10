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
        /// <param name="parent">A panel builder.</param>
        /// <param name="name">Internal name of the button.</param>
        /// <param name="builder">The button builder.</param>
        /// <typeparam name="TCommand">TClass which implements IExternalCommand interface.
        /// This command will be execute when user push the button.</typeparam>
        public static IPanelBuilder CommandButton<TCommand>(
            this IPanelBuilder parent,
            string name,
            Action<ICommandButtonBuilder>? builder = null)
        {
            return parent.CommandButton(name, typeof(TCommand), builder);
        }

        /// <summary>
        /// Adds a new push button to the panel, with button name of the command type full name.
        /// </summary>
        /// <param name="parent">A panel builder.</param>
        /// <param name="builder">The button builder.</param>
        /// <typeparam name="TCommand">TClass which implements IExternalCommand interface.
        /// This command will be execute when user push the button.</typeparam>
        public static IPanelBuilder CommandButton<TCommand>(
            this IPanelBuilder parent,
            Action<ICommandButtonBuilder>? builder = null)
        {
            var commandType = typeof(TCommand);
            return parent.CommandButton(GetDefaultCommandName(commandType), commandType, builder);
        }

        /// <summary>
        /// Adds a new toggle command button to the panel.
        /// </summary>
        /// <param name="parent">A panel builder.</param>
        /// <param name="name">Internal name of the button.</param>
        /// <param name="builder">The button builder.</param>
        /// <typeparam name="TCommand">Command type executed when the button is clicked.</typeparam>
        public static IPanelBuilder ToggleCommandButton<TCommand>(
            this IPanelBuilder parent,
            string name,
            Action<IToggleCommandButtonBuilder>? builder = null)
        {
            return parent.ToggleCommandButton(name, typeof(TCommand), builder);
        }

        /// <summary>
        /// Adds a new toggle command button to the panel, with button name of the command type full name.
        /// </summary>
        /// <param name="parent">A panel builder.</param>
        /// <param name="builder">The button builder.</param>
        /// <typeparam name="TCommand">Command type executed when the button is clicked.</typeparam>
        public static IPanelBuilder ToggleCommandButton<TCommand>(
            this IPanelBuilder parent,
            Action<IToggleCommandButtonBuilder>? builder = null)
        {
            var commandType = typeof(TCommand);
            return parent.ToggleCommandButton(GetDefaultCommandName(commandType), commandType, builder);
        }

        /// <summary>
        /// Adds a new push button to the panel.
        /// </summary>
        /// <param name="parent">A panel builder.</param>
        /// <param name="name">Internal name of the button.</param>
        /// <param name="builder">The button builder.</param>
        /// <typeparam name="TCommand">TClass which implements IExternalCommand interface.
        /// This command will be execute when user push the button.</typeparam>
        public static IPulldownButtonBuilder CommandButton<TCommand>(
            this IPulldownButtonBuilder parent,
            string name,
            Action<ICommandButtonBuilder>? builder = null)
        {
            return parent.CommandButton(name, typeof(TCommand), builder);
        }

        /// <summary>
        /// Adds a new push button to the panel, with button name of the command type full name.
        /// </summary>
        /// <param name="parent">A panel builder.</param>
        /// <param name="builder">The button builder.</param>
        /// <typeparam name="TCommand">TClass which implements IExternalCommand interface.
        /// This command will be execute when user push the button.</typeparam>
        public static IPulldownButtonBuilder CommandButton<TCommand>(
            this IPulldownButtonBuilder parent,
            Action<ICommandButtonBuilder>? builder = null)
        {
            var commandType = typeof(TCommand);
            return parent.CommandButton(GetDefaultCommandName(commandType), commandType, builder);
        }

        /// <summary>
        /// Adds a new push button to the panel.
        /// </summary>
        /// <param name="parent">A panel builder.</param>
        /// <param name="name">Internal name of the button.</param>
        /// <param name="builder">The button builder.</param>
        /// <typeparam name="TCommand">TClass which implements IExternalCommand interface.
        /// This command will be execute when user push the button.</typeparam>
        public static IStackedItemsBuilder CommandButton<TCommand>(
            this IStackedItemsBuilder parent,
            string name,
            Action<ICommandButtonBuilder>? builder = null)
        {
            return parent.CommandButton(name, typeof(TCommand), builder);
        }

        /// <summary>
        /// Adds a new push button to the panel, with button name of the command type full name.
        /// </summary>
        /// <param name="parent">A panel builder.</param>
        /// <param name="builder">The button builder.</param>
        /// <typeparam name="TCommand">TClass which implements IExternalCommand interface.
        /// This command will be execute when user push the button.</typeparam>
        public static IStackedItemsBuilder CommandButton<TCommand>(
            this IStackedItemsBuilder parent,
            Action<ICommandButtonBuilder>? builder = null)
        {
            var commandType = typeof(TCommand);
            return parent.CommandButton(GetDefaultCommandName(commandType), commandType, builder);
        }

        /// <summary>
        /// Adds a new toggle command button to the stack.
        /// </summary>
        /// <param name="parent">A stacked items builder.</param>
        /// <param name="name">Internal name of the button.</param>
        /// <param name="builder">The button builder.</param>
        /// <typeparam name="TCommand">Command type executed when the button is clicked.</typeparam>
        public static IStackedItemsBuilder ToggleCommandButton<TCommand>(
            this IStackedItemsBuilder parent,
            string name,
            Action<IToggleCommandButtonBuilder>? builder = null)
        {
            return parent.ToggleCommandButton(name, typeof(TCommand), builder);
        }

        /// <summary>
        /// Adds a new toggle command button to the stack, with button name of the command type full name.
        /// </summary>
        /// <param name="parent">A stacked items builder.</param>
        /// <param name="builder">The button builder.</param>
        /// <typeparam name="TCommand">Command type executed when the button is clicked.</typeparam>
        public static IStackedItemsBuilder ToggleCommandButton<TCommand>(
            this IStackedItemsBuilder parent,
            Action<IToggleCommandButtonBuilder>? builder = null)
        {
            var commandType = typeof(TCommand);
            return parent.ToggleCommandButton(GetDefaultCommandName(commandType), commandType, builder);
        }

        /// <summary>
        /// Loads command button properties from <see cref="RxBimCommandAttribute"/>.
        /// </summary>
        /// <param name="button">A Command button.</param>
        /// <param name="assembly">An assembly to load from.</param>
        public static void LoadFromAttribute(this CommandButton button, Assembly assembly)
        {
            LoadFromAttribute(button, button.CommandType, assembly);
        }

        /// <summary>
        /// Loads toggle command button properties from <see cref="RxBimCommandAttribute"/>.
        /// </summary>
        /// <param name="button">A toggle command button.</param>
        /// <param name="assembly">An assembly to load from.</param>
        public static void LoadFromAttribute(this ToggleCommandButton button, Assembly assembly)
        {
            LoadFromAttribute(button, button.CommandType, assembly);
        }

        private static void LoadFromAttribute(Button button, string? commandTypeName, Assembly assembly)
        {
            if (commandTypeName is null)
                return;

            var commandType = assembly.GetTypeByName(commandTypeName);
            var attr = commandType.GetCustomAttribute<RxBimCommandAttribute>(true);
            if (attr == null)
                return;

            button.Description ??= attr.Description;
            button.Text ??= attr.Text;
            button.ToolTip ??= attr.ToolTip;
            button.HelpUrl ??= attr.HelpUrl;
            button.Image ??= attr.Image;
            button.LargeImage ??= attr.LargeImage;
            button.ImageLight ??= attr.SmallImageLight ?? attr.Image;
            button.LargeImageLight ??= attr.LargeImageLight ?? attr.LargeImage;
        }

        private static string GetDefaultCommandName(Type commandType)
        {
            return commandType.FullName ?? commandType.Name;
        }
    }
}