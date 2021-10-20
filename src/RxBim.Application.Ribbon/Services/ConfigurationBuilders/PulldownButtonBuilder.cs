namespace RxBim.Application.Ribbon.Services.ConfigurationBuilders
{
    using System;
    using Abstractions.ConfigurationBuilders;
    using Models.Configurations;

    /// <summary>
    /// PulldownButtonBuilder
    /// </summary>
    public class PulldownButtonBuilder : ButtonBuilder<PullDownButton>, IPulldownButtonBuilder
    {
        /// <inheritdoc />
        public PulldownButtonBuilder(string name, string text)
            : base(name, text)
        {
        }

        /// <inheritdoc/>
        public IPulldownButtonBuilder AddCommandButton(
            string name,
            string text,
            Type commandType,
            Action<ICommandButtonBuilder>? action = null)
        {
            var buttonBuilder = new CommandButtonBuilder(name, text, commandType);
            action?.Invoke(buttonBuilder);
            BuildingButton.Buttons.Add(buttonBuilder.BuildingButton);
            return this;
        }
    }
}