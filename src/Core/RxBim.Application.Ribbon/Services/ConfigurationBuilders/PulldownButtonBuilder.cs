namespace RxBim.Application.Ribbon.ConfigurationBuilders
{
    using System;

    /// <summary>
    /// Represents a pull-down builder.
    /// </summary>
    public class PulldownButtonBuilder : ButtonBuilder<PullDownButton, IPulldownButtonBuilder>, IPulldownButtonBuilder
    {
        /// <inheritdoc />
        public PulldownButtonBuilder(string name)
            : base(name)
        {
        }

        /// <inheritdoc/>
        public IPulldownButtonBuilder CommandButton(
            string name,
            Type commandType,
            Action<ICommnadButtonBuilder>? builder = null)
        {
            var buttonBuilder = new CommandButtonBuilder(name, commandType);
            ////buttonBuilder.BuildFromAttribute(commandType);
            builder?.Invoke(buttonBuilder);
            BuildingButton.CommandButtonsList.Add(buttonBuilder.BuildingButton);
            return this;
        }
    }
}