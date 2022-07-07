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
            Action<ICommandButtonBuilder>? builder = null)
        {
            var buttonBuilder = new CommandButtonBuilder(name, commandType);
            builder?.Invoke(buttonBuilder);
            Button.CommandButtonsList.Add(buttonBuilder.Build());
            return this;
        }
    }
}