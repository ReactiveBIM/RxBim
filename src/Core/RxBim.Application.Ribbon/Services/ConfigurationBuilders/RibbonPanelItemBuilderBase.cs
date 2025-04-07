namespace RxBim.Application.Ribbon.ConfigurationBuilders
{
    /// <summary>
    /// Base implementation of a ribbon panel item builder.
    /// </summary>
    public abstract class RibbonPanelItemBuilderBase<TItem, TItemBuilder> : IRibbonPanelItemBuilder<TItemBuilder>
        where TItem : RibbonPanelItemBase, new()
        where TItemBuilder : class, IRibbonPanelItemBuilder<TItemBuilder>
    {
        /// <summary>
        /// Initializes a new instance of the ribbon panel item builder.
        /// </summary>
        /// <param name="name">Item name.</param>
        protected RibbonPanelItemBuilderBase(string name)
        {
            Item.Name = name;
        }

        /// <summary>
        /// Item to create configuration.
        /// </summary>
        protected TItem Item { get; } = new();

        /// <inheritdoc />
        public virtual TItemBuilder Image(string imageRelativePath, ThemeType theme = ThemeType.All)
        {
            Item.Image = imageRelativePath;
            return (this as TItemBuilder)!;
        }

        /// <inheritdoc />
        public TItemBuilder Description(string description)
        {
            Item.Description = description;
            return (this as TItemBuilder)!;
        }

        /// <inheritdoc />
        public TItemBuilder ToolTip(string toolTip)
        {
            Item.ToolTip = toolTip;
            return (this as TItemBuilder)!;
        }

        /// <inheritdoc />
        public TItemBuilder Text(string text)
        {
            Item.Text = text;
            return (this as TItemBuilder)!;
        }

        /// <summary>
        /// Returns button.
        /// </summary>
        internal TItem Build()
        {
            return Item;
        }
    }
}