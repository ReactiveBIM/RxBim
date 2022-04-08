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
        public PulldownButtonBuilder(string name)
            : base(name)
        {
        }

        /// <inheritdoc/>
        public IPulldownButtonBuilder AddCommandButton(
            string name,
            Type commandType,
            Action<IButtonBuilder>? action = null)
        {
            var buttonBuilder = new CommandButtonBuilder(name, commandType);
            action?.Invoke(buttonBuilder);
            BuildingButton.CommandButtonsList.Add(buttonBuilder.BuildingButton);
            return this;
        }

        /// <inheritdoc/>
        public IPulldownButtonBuilder AddCommandButton<T>(string name, Action<IButtonBuilder>? action = null)
        {
            return AddCommandButton(name, typeof(T), action);
        }
    }
}