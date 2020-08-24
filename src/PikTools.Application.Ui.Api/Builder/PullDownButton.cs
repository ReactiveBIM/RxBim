namespace PikTools.Application.Ui.Api.Builder
{
    using System;
    using System.Collections.Generic;
    using Autodesk.Revit.UI;

    /// <inheritdoc />
    public class PulldownButton : Button
    {
        /// <inheritdoc />
        public PulldownButton(string name, string text)
            : base(name, text, null)
        {
        }

        private IList<Button> Buttons { get; } = new List<Button>();

        /// <summary>
        /// Create push button on the panel
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <returns>Panel where button were created</returns>
        public PulldownButton Button<TExternalCommandClass>(string name, string text)
            where TExternalCommandClass : class, IExternalCommand
        {
            var commandClassType = typeof(TExternalCommandClass);

            return Button(name, text, commandClassType, null);
        }

        /// <summary>
        /// Create push button on the panel
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <param name="action">Additional action with whe button</param>
        /// <returns>Panel where button were created</returns>
        public PulldownButton Button<TExternalCommandClass>(string name, string text, Action<Button> action)
            where TExternalCommandClass : class, IExternalCommand
        {
            var commandClassType = typeof(TExternalCommandClass);

            return Button(name, text, commandClassType, action);
        }

        /// <summary>
        /// Create push button on the panel
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <param name="externalCommandType">Class which implements IExternalCommand interface. 
        /// This command will be execute when user push the button</param>
        /// <returns>Panel where button were created</returns>
        public PulldownButton Button(string name, string text, Type externalCommandType)
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
        public PulldownButton Button(string name, string text, Type externalCommandType, Action<Button> action)
        {
            var button = new Button(name, text, externalCommandType);
            action?.Invoke(button);

            Buttons.Add(button);

            return this;
        }

        /// <inheritdoc/>
        internal override ButtonData Finish()
        {
            PulldownButtonData pulldownButtonData = new PulldownButtonData(Name, Text);

            if (LargeImage != null)
            {
                pulldownButtonData.LargeImage = LargeImage;
            }

            if (SmallImage != null)
            {
                pulldownButtonData.Image = SmallImage;
            }

            if (Description != null)
            {
                pulldownButtonData.LongDescription = Description;
            }

            if (ContextualHelp != null)
            {
                pulldownButtonData.SetContextualHelp(ContextualHelp);
            }

            return pulldownButtonData;
        }

        /// <summary>
        /// BuildButtons
        /// </summary>
        /// <param name="pulldownButton">PulldownButton</param>
        internal void BuildButtons(Autodesk.Revit.UI.PulldownButton pulldownButton)
        {
            foreach (var button in Buttons)
            {
                pulldownButton.AddPushButton(button.Finish() as PushButtonData);
            }
        }
    }
}