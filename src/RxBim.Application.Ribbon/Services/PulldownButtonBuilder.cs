namespace RxBim.Application.Ribbon.Services
{
    using System;
    using Abstractions;
    using Models;

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
            Control.Buttons.Add(buttonBuilder.Control);
            return this;
        }
    }
}