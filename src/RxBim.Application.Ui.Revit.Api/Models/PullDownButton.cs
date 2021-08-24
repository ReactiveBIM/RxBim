namespace RxBim.Application.Ui.Revit.Api.Models
{
    using System;
    using System.Collections.Generic;
    using Autodesk.Revit.UI;
    using RxBim.Application.Ui.Api.Abstractions;

    /// <summary>
    /// PulldownButton
    /// </summary>
    public class PulldownButton : Button, IPulldownButton
    {
        private readonly IList<Button> _buttons = new List<Button>();

        /// <inheritdoc />
        public PulldownButton(string name, string text)
            : base(name, text, null)
        {
        }

        /// <summary>
        /// Create push button on the panel
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <param name="externalCommandType">Class which implements IExternalCommand interface.
        /// This command will be execute when user push the button</param>
        /// <returns>Panel where button were created</returns>
        public IPulldownButton Button(string name, string text, Type externalCommandType)
        {
            return Button(name, text, externalCommandType, null);
        }

        /// <summary>
        /// Create push button on the panel
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <param name="externalCommandType">Class which implements IExternalCommand interface.
        /// This command will be execute when user push the button</param>
        /// <param name="action">Additional action with whe button</param>
        /// <returns>Panel where button were created</returns>
        public IPulldownButton Button(string name, string text, Type externalCommandType, Action<IButton> action)
        {
            var button = new Button(name, text, externalCommandType);
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