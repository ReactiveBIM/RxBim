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
        public IStackedItemsBuilder CommandButton(
            string name,
            Type commandType,
            Action<ICommnadButtonBuilder>? builder = null)
        {
            var buttonBuilder = new CommandButtonBuilder(name, commandType);
            builder?.Invoke(buttonBuilder);

            return AddItem(buttonBuilder.BuildingButton);
        }

        /// <inheritdoc />
        public IStackedItemsBuilder PullDownButton(string name, Action<IPulldownButtonBuilder> builder)
        {
            var pulldownButton = new PulldownButtonBuilder(name);
            builder.Invoke(pulldownButton);
            return AddItem(pulldownButton.BuildingButton);
        }

        /// <summary>
        /// Loads buttons from configurations.
        /// </summary>
        /// <param name="stackedButtons">A buttons config section.</param>
        internal void LoadFromConfig(IConfigurationSection stackedButtons)
        {
            foreach (var buttonSection in stackedButtons.GetChildren())
            {
                if (!buttonSection.Exists())
                    continue;

                if (buttonSection.GetSection(nameof(Application.Ribbon.CommandButton.CommandType)).Exists())
                {
                    LoadFromConfig<CommandButton>(buttonSection);
                }
                else if (buttonSection.GetSection(nameof(Application.Ribbon.PullDownButton.CommandButtonsList)).Exists())
                {
                    LoadFromConfig<PullDownButton>(buttonSection);
                }
            }
        }

        private StackedItemsBuilder AddItem(Button item)
        {
            if (StackedItems.StackedButtons.Count == MaxStackSize)
                throw new InvalidOperationException("You cannot create more than three items in the StackedItem");
            StackedItems.StackedButtons.Add(item);
            return this;
        }

        private void LoadFromConfig<T>(IConfiguration buttonSection)
            where T : Button
        {
            var item = buttonSection.Get<T>();
            AddItem(item);
        }
    }
}