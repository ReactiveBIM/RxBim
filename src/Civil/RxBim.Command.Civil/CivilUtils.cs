namespace RxBim.Shared.Civil;

using System;
using System.Linq;

/// <summary>
/// Utils for Civil 3D.
/// </summary>
public static class CivilUtils
{
    /// <summary>
    /// Returns true if Civil 3D API are supported.
    /// </summary>
    public static bool IsCivilSupported()
    {
        try
        {
            const string civilModuleName = "AeccDbMgd";
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            return assemblies.Any(assembly => assembly.FullName.Contains(civilModuleName));
        }
        catch
        {
            return false;
        }
    }
}