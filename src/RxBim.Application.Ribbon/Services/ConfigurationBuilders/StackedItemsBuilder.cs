namespace RxBim.Application.Ribbon.ConfigurationBuilders
{
    using System;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Represents an items stack builder.
    /// </summary>
    public class StackedItemsBuilder : IStackedItemsBuilder
    {
        /// <summary>
        /// Max size of a stack.
        /// </summary>
        public const int MaxStackSize = 3;

        /// <summary>
        /// Builds StackedItems.
        /// </summary>
        public StackedItems StackedItems { get; } = new();

        /// <inheritdoc />
        public IStackedItemsBuilder AddCommandButton(
            string name,
            Type commandType,
            Action<IButtonBuilder>? builder = null)
        {
            var buttonBuilder = new CommandButtonBuilder(name, commandType);
            builder?.Invoke(buttonBuilder);

            return AddButton(buttonBuilder.BuildingButton);
        }

        /// <inheritdoc />
        public IStackedItemsBuilder AddPullDownButton(string name, Action<IPulldownButtonBuilder> builder)
        {
            var pulldownButton = new PulldownButtonBuilder(name);
            builder.Invoke(pulldownButton);
            return AddButton(pulldownButton.BuildingButton);
        }

        /// <summary>
        /// Loads buttons from configurations.
        /// </summary>
        /// <param name="stackedButtons">A buttons config section.</param>
        internal void LoadButtonsFromConfig(IConfigurationSection stackedButtons)
        {
            foreach (var buttonSection in stackedButtons.GetChildren())
            {
                if (!buttonSection.Exists())
                    continue;

                if (buttonSection.GetSection(nameof(CommandButton.CommandType)).Exists())
                {
                    AddButton<CommandButton>(buttonSection);
                }
                else if (buttonSection.GetSection(nameof(PullDownButton.CommandButtonsList)).Exists())
                {
                    AddButton<PullDownButton>(buttonSection);
                }
            }
        }

        private StackedItemsBuilder AddButton(Button button)
        {
            if (StackedItems.StackedButtons.Count == MaxStackSize)
                throw new InvalidOperationException("You cannot create more than three items in the StackedItem");
            StackedItems.StackedButtons.Add(button);
            return this;
        }

        private void AddButton<T>(IConfiguration buttonSection)
            where T : Button
        {
            var button = buttonSection.Get<T>();
            AddButton(button);
        }
    }
}