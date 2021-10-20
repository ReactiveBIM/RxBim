namespace RxBim.Application.Ribbon.Revit.Models
{
    using System;
    using System.Collections.Generic;
    using Abstractions;
    using Abstractions.ConfigurationBuilders;
    using Autodesk.Revit.UI;

    /// <summary>
    /// PulldownButton
    /// </summary>
    public class PulldownButtonBuilder : ButtonBuilder, IPulldownButtonBuilder
    {
        private readonly IList<ButtonBuilder> _buttons = new List<ButtonBuilder>();

        /// <inheritdoc />
        public PulldownButtonBuilder(string name, string text)
            : base(name, text, null)
        {
        }

        /// <summary>
        /// Create push button on the panel
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <param name="commandType">Class which implements IExternalCommand interface.
        /// This command will be execute when user push the button</param>
        /// <returns>Panel where button were created</returns>
        public IPulldownButtonBuilder AddCommandButton(string name, string text, Type commandType)
        {
            return AddCommandButton(name, text, commandType, null);
        }

        /// <summary>
        /// Create push button on the panel
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <param name="commandType">Class which implements IExternalCommand interface.
        /// This command will be execute when user push the button</param>
        /// <param name="action">Additional action with whe button</param>
        /// <returns>Panel where button were created</returns>
        public IPulldownButtonBuilder AddCommandButton(string name, string text, Type commandType, Action<IButtonBuilder> action)
        {
            var button = new ButtonBuilder(name, text, commandType);
            action?.Invoke(button);
            _buttons.Add(button);
            return this;
        }

        /// <inheritdoc/>
        internal override ButtonData GetButtonData()
        {
            var pulldownButtonData = new PulldownButtonData(Name, Text);
            SetBaseProperties(pulldownButtonData);
            return pulldownButtonData;
        }

        /// <summary>
        /// BuildButtons
        /// </summary>
        /// <param name="pulldownButton">PulldownButton</param>
        internal void BuildButtons(Autodesk.Revit.UI.PulldownButton pulldownButton)
        {
            foreach (var button in _buttons)
            {
                pulldownButton.AddPushButton(button.GetButtonData() as PushButtonData);
            }
        }
    }
}