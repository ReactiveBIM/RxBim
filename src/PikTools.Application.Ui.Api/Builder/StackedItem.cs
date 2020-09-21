namespace PikTools.Application.Ui.Api.Builder
{
    using System;
    using System.Collections.Generic;
    using Autodesk.Revit.UI;

    /// <summary>
    /// StackedItem
    /// </summary>
    public class StackedItem
    {
        private readonly Panel _panel;
        private readonly IList<Button> _buttons;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="panel">panel</param>
        public StackedItem(Panel panel)
        {
            _panel = panel;
            _buttons = new List<Button>(3);
        }

        /// <summary>
        /// Items count
        /// </summary>
        public int ItemsCount => Buttons.Count;

        /// <summary>
        /// Buttons
        /// </summary>
        public IList<Button> Buttons => _buttons;

        /// <summary>
        /// Create push button on the panel
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <returns>Panel where button were created</returns>
        public StackedItem Button<TExternalCommandClass>(string name, string text)
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
        public StackedItem Button<TExternalCommandClass>(string name, string text, Action<Button> action)
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
        public StackedItem Button(string name, string text, Type externalCommandType)
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
        public StackedItem Button(string name, string text, Type externalCommandType, Action<Button> action)
        {
            if (Buttons.Count == 3)
            {
                throw new InvalidOperationException("You cannot create more than three items in the StackedItem");
            }

            var button = new Button(name, text, externalCommandType);
            action?.Invoke(button);

            Buttons.Add(button);

            return this;
        }
    }
}