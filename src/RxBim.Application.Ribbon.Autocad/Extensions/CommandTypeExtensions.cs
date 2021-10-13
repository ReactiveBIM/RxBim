namespace RxBim.Application.Ribbon.Autocad.Extensions
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Extensions for command type
    /// </summary>
    internal static class CommandTypeExtensions
    {
        /// <summary>
        /// Returns command name from command type
        /// </summary>
        /// <param name="commandType">Command type</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if an undefined error occurred while getting the command name
        /// </exception>
        public static string GetCommandName(this Type commandType)
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