namespace RxBim.Application.Ribbon
{
    /// <summary>
    /// Defines a base button builder.
    /// </summary>
    /// <typeparam name="TButton">The type of a button.</typeparam>
    /// <typeparam name="TButtonBuilder">The type of button builder.</typeparam>
    public interface IButtonBuilderBase<out TButton, out TButtonBuilder> : IButtonBuilder<TButtonBuilder>
        where TButton : Button, new()
        where TButtonBuilder : class, IButtonBuilder<TButtonBuilder>
    {
        /// <summary>
        /// The button to create configuration.
        /// </summary>
        public TButton BuildingButton { get; }
    }
}