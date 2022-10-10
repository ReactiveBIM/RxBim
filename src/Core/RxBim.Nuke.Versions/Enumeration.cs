namespace RxBim.Nuke.Versions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Implementation of the base enumeration.
    /// </summary>
    public abstract record Enumeration
    {
        /// <summary>
        /// Returns all members of the enumeration.
        /// </summary>
        /// <typeparam name="T">Type of the enumeration member.</typeparam>
        public static IEnumerable<T> GetAll<T>()
            where T : Enumeration =>
            typeof(T).GetFields(BindingFlags.Public |
                                BindingFlags.Static |
                                BindingFlags.DeclaredOnly)
                .Select(f => f.GetValue(null))
                .Cast<T>();
    }
}