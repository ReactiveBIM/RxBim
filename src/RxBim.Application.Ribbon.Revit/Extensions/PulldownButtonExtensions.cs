namespace RxBim.Application.Ribbon.Revit.Extensions
{
    using System;
    using Abstractions;
    using Abstractions.ConfigurationBuilders;
    using Autodesk.Revit.UI;

    /// <summary>
    /// Расширения для PulldownButton
    /// </summary>
    public static class PulldownButtonExtensions
    {
        /// <summary>
        /// Create push button on the panel
        /// </summary>
        /// <param name="pulldownButtonBuilder">PulldownButton</param>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <param name="action">Additional action with whe button</param>
        /// <returns>Panel where button were created</returns>
        public static IButtonBuilder Button<TExternalCommandClass>(
            this IPulldownButtonBuilder pulldownButtonBuilder,
            string name,
            string text,
            Action<IButtonBuilder> action = null)
            where TExternalCommandClass : class, IExternalCommand
        {
            var commandClassType = typeof(TExternalCommandClass);
            return pulldownButtonBuilder.AddCommandButton(name, text, commandClassType, action);
        }
    }
}