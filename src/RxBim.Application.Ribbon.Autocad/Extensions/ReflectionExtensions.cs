namespace RxBim.Application.Ribbon.Autocad.Extensions
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Extensions for reflection objects
    /// </summary>
    internal static class ReflectionExtensions
    {
        /// <summary>
        /// Returns command name from command class type
        /// </summary>
        /// <param name="commandType">Command class type</param>
        /// <exception cref="InvalidOperationException">If command class type is not valid</exception>
        public static string GetCommandName(this MemberInfo commandType)
        {
            const string cmdNameProperty = "CommandName";
            var attributes = Attribute.GetCustomAttributes(commandType);

            foreach (var attribute in attributes)
            {
                try
                {
                    var cmdProperty = attribute.GetType()
                        .GetProperty(cmdNameProperty,
                            BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

                    if (cmdProperty is null)
                        continue;

                    return cmdProperty.GetValue(attribute).ToString();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new InvalidOperationException("Failed to retrieve command name!", e);
                }
            }

            return commandType.Name;
        }
    }
}