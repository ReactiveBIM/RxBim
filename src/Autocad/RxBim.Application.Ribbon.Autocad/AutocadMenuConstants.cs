namespace RxBim.Application.Ribbon
{
    /// <summary>
    /// Constants for the AutoCAD menu.
    /// </summary>
    internal static class AutocadMenuConstants
    {
        /// <summary>
        /// Variable name for specified workspace to be the current one.
        /// </summary>
        public const string WorkSpaceVariableName = "WSCURRENT";

        /// <summary>
        /// Variable name for the color theme of the ribbon.
        /// </summary>
        public const string ColorThemeVariableName = "COLORTHEME";

        /// <summary>
        /// Variable name to suppress the message display (muttering).
        /// </summary>
        public const string MuterringVariableName = "NOMUTT";

        /// <summary>
        /// Value of variable with name <see cref="MuterringVariableName"/> for suppresses muttering.
        /// </summary>
        public const short MuterringOffValue = 1;
    }
}