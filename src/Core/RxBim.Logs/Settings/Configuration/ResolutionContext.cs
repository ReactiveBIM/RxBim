#pragma warning disable SA1600
namespace RxBim.Logs.Settings.Configuration
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Configuration;
    using Serilog.Core;

    /// <summary>
    /// Keeps track of available elements that are useful when resolving values in the settings system.
    /// </summary>
    internal sealed class ResolutionContext
    {
        private readonly IDictionary<string, LoggingLevelSwitch> _declaredLevelSwitches;
        private readonly IConfiguration? _appConfiguration;

        public ResolutionContext(IConfiguration? appConfiguration = null)
        {
            _declaredLevelSwitches = new Dictionary<string, LoggingLevelSwitch>();
            _appConfiguration = appConfiguration;
        }

        public bool HasAppConfiguration => _appConfiguration != null;

        public IConfiguration? AppConfiguration
        {
            get
            {
                if (!HasAppConfiguration)
                {
                    throw new InvalidOperationException("AppConfiguration is not available");
                }

                return _appConfiguration;
            }
        }

        /// <summary>
        /// Looks up a switch in the declared LoggingLevelSwitches.
        /// </summary>
        /// <param name="switchName">the name of a switch to look up.</param>
        /// <returns>the LoggingLevelSwitch registered with the name.</returns>
        /// <exception cref="InvalidOperationException">if no switch has been registered with <paramref name="switchName"/>.</exception>
        public LoggingLevelSwitch? LookUpSwitchByName(string? switchName)
        {
            if (switchName is null)
                return null;

            return _declaredLevelSwitches.TryGetValue(switchName, out var levelSwitch)
                ? levelSwitch
                : throw new InvalidOperationException(
                    $"No LoggingLevelSwitch has been declared with name \"{switchName}\". You might be missing a section \"LevelSwitches\":{{\"{switchName}\":\"InitialLevel\"}}");
        }

        public void AddLevelSwitch(string? levelSwitchName, LoggingLevelSwitch levelSwitch)
        {
            if (levelSwitchName is null)
                throw new ArgumentNullException(nameof(levelSwitchName));

            _declaredLevelSwitches[levelSwitchName] = levelSwitch ?? throw new ArgumentNullException(nameof(levelSwitch));
        }
    }
}