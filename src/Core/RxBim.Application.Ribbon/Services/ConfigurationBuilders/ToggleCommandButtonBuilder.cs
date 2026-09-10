namespace RxBim.Application.Ribbon.ConfigurationBuilders
{
    using System;

    /// <summary>
    /// Represents a toggle command button builder.
    /// </summary>
    public class ToggleCommandButtonBuilder : ButtonBuilder<ToggleCommandButton, IToggleCommandButtonBuilder>, IToggleCommandButtonBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToggleCommandButtonBuilder"/> class.
        /// </summary>
        /// <param name="name">The button name.</param>
        /// <param name="commandType">The command type.</param>
        public ToggleCommandButtonBuilder(string name, Type commandType)
            : base(name)
        {
            Item.CommandType = commandType.AssemblyQualifiedName;
        }

        /// <inheritdoc />
        public IToggleCommandButtonBuilder IsChecked(bool isChecked = true)
        {
            Item.IsChecked = isChecked;
            return this;
        }

        /// <inheritdoc />
        public IToggleCommandButtonBuilder IsThreeState(bool isThreeState = true)
        {
            Item.IsThreeState = isThreeState;
            return this;
        }
    }
}