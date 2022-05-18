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
            Action<ICommnadButtonBuilder>? builder = null)
        {
            return parent.CommandButton(name, typeof(TCommand), builder);
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
            Action<ICommnadButtonBuilder>? builder = null)
        {
            return parent.CommandButton(name, typeof(TCommand), builder);
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
            Action<ICommnadButtonBuilder>? builder = null)
        {
            return parent.CommandButton(name, typeof(TCommand), builder);
        }

        /// <summary>
        /// Loads command button properties from <see cref="RxBimCommandAttribute"/>.
        /// </summary>
        /// <param name="button">A Command button.</param>
        /// <param name="assembly">An assembly to load from.</param>
        internal static void LoadFromAttribute(this IRibbonPanelElement button, Assembly assembly)
        {
            switch (button)
            {
                case CommandButton cmd: cmd.LoadFromAttributeInternal(assembly); break;
                case PullDownButton pulldown: pulldown.CommandButtonsList.ForEach(x => x.LoadFromAttribute(assembly)); break;
                case StackedItems stacked: stacked.StackedButtons.ForEach(x => x.LoadFromAttribute(assembly)); break;
            }
        }
        
        /// <summary>
        /// Loads command button properties from <see cref="RxBimCommandAttribute"/>.
        /// </summary>
        /// <param name="button">A Command button.</param>
        /// <param name="assembly">An assembly to load from.</param>
        private static void LoadFromAttributeInternal(this CommandButton button, Assembly assembly)
        {
            if (button.CommandType is null)
                return;
            var commandType = AssemblyExtensions.GetTypeByName(assembly, button.CommandType);
            var attr = commandType.GetCustomAttribute<RxBimCommandAttribute>(true);
            if (attr == null)
                return;

            button.Description ??= attr.Description!;
            button.Text ??= attr.Text!;
            button.ToolTip ??= attr.ToolTip!;
            button.HelpUrl ??= attr.HelpUrl!;
            button.SmallImage ??= attr.SmallImage!;
            button.LargeImage ??= attr.LargeImage!;
            button.SmallImageLight ??= attr.SmallImageLight ?? attr.SmallImage!;
            button.LargeImageLight ??= attr.LargeImageLight ?? attr.LargeImage!;
        }
    }
}