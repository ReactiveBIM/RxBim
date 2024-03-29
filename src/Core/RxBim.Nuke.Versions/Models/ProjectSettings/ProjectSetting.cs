﻿namespace RxBim.Nuke.Versions
{
    /// <summary>
    /// The base class for the project setting.
    /// </summary>
    /// <param name="Name">Setting name.</param>
    /// <param name="Value">Setting value.</param>
    /// <param name="Type">Setting type.</param>
    /// <param name="Attributes">Setting attributes collection.</param>
    public record ProjectSetting(
        string Name,
        string Value,
        ProjectSettingType Type,
        params ProjectSettingAttribute[] Attributes);
}