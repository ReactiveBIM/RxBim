namespace RxBim.Application.Ribbon.Models
{
    using System;
    using System.Collections.Generic;
    using Abstractions;

    /// <summary>
    /// StackedItem
    /// </summary>
    public abstract class StackedItemBase<TButton> : IStackedItem
        where TButton : IButton
    {
        /// <summary>
        /// Items count
        /// </summary>
        public int ItemsCount => Buttons.Count;

        /// <summary>
        /// Buttons
        /// </summary>
        public IList<TButton> Buttons { get; } = new List<TButton>(3);

        /// <summary>
        /// Create push button on the panel
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <param name="externalCommandType">Class which implements IExternalCommand interface.
        /// This command will be execute when user push the button</param>
        /// <param name="action">Additional action with whe button</param>
        /// <returns>Panel where button were created</returns>
        public IStackedItem Button(string name, string text, Type externalCommandType, Action<IButton> action = null)
        {
            if (Buttons.Count == 3)
            {
                throw new InvalidOperationException("You cannot create more than three items in the StackedItem");
            }

            var button = GetButton(name, text, externalCommandType);
            action?.Invoke(button);

            Buttons.Add(button);

            return this;
        }

        /// <summary>
        /// Возвращает кнопку
        /// </summary>
        /// <param name="name">Название</param>
        /// <param name="text">Текст</param>
        /// <param name="externalCommandType">Тип команды</param>
        protected abstract TButton GetButton(string name, string text, Type externalCommandType);
    }
}