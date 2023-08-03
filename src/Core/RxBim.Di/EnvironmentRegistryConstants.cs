namespace RxBim.Di
{
    /// <summary>
    /// Constants for environment registry.
    /// </summary>
    public static class EnvironmentRegistryConstants
    {
        /// <summary>
        /// Registry entry for plugins environments.
        /// </summary>
        public const string RxBimEnvironmentRegPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\RxBimEnvironments";

        /// <summary>
        /// Name of environment key.
        /// </summary>
        public const string EnvironmentRegKeyName = "Environment";

        /// <summary>
        /// Name of plugin name key.
        /// </summary>
        public const string PluginNameRegKeyName = "PluginName";

        /// <summary>
        /// Default environment.
        /// </summary>
        public const string DefaultEnvironment = "Testing";
    }
}