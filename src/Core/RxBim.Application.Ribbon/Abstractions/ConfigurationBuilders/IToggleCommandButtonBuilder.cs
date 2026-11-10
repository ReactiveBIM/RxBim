namespace RxBim.Application.Ribbon
{
    /// <summary>
    /// Defines a builder for <see cref="ToggleCommandButton"/>.
    /// </summary>
    public interface IToggleCommandButtonBuilder : IButtonBuilder<IToggleCommandButtonBuilder>
    {
        /// <summary>
        /// Sets the initial checked state.
        /// </summary>
        /// <param name="isChecked">Checked state.</param>
        IToggleCommandButtonBuilder IsChecked(bool isChecked = true);

        /// <summary>
        /// Sets whether the button supports three states.
        /// </summary>
        /// <param name="isThreeState">Three-state mode flag.</param>
        IToggleCommandButtonBuilder IsThreeState(bool isThreeState = true);
    }
}