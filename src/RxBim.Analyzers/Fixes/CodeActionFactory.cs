namespace RxBim.Analyzers.Fixes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Factory for creating action for code fixes
    /// </summary>
    public class CodeActionFactory
    {
        private static readonly Dictionary<string, IActionCreator> Actions;

        static CodeActionFactory()
        {
            Actions = typeof(IActionCreator).Assembly
                .GetTypes()
                .Where(type => !type.IsAbstract && type.GetInterfaces().Any(i => i == typeof(IActionCreator)))
                .Select(Activator.CreateInstance)
                .Cast<IActionCreator>()
                .ToDictionary(x => x.DiagnosticId);
        }

        /// <summary>
        /// Creates code fix action
        /// </summary>
        /// <param name="id">diagnostic identifier</param>
        public static IActionCreator GetCreator(string id)
        {
            return Actions.TryGetValue(id, out var actionCreator) ? actionCreator : throw new ArgumentException();
        }
    }
}