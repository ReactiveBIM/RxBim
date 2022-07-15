namespace RxBim.Nuke.Helpers
{
    extern alias nc;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using nc::System.Runtime.CompilerServices;

    /// <summary>
    /// Contains validation methods.
    /// </summary>
    public static class ArgumentHelper
    {
        /// <summary>
        /// Checks the value is not null.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="name">The caller argument name.</param>
        /// <typeparam name="T">The value type.</typeparam>
        /////[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Ensure<T>([NotNull] this T? value, [CallerArgumentExpression("value")] string name = "")
        {
            if (value == null)
                throw new NullReferenceException($"Value of {name} cannot be null.");

            return value;
        }
    }
}