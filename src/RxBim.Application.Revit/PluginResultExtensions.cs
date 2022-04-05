// ReSharper disable once CheckNamespace
namespace RxBim.Shared
{
    /// <summary>
    /// Extensions for <see cref="PluginResult"/>.
    /// </summary>
    public static class PluginResultExtensions
    {
        /// <summary>
        /// Maps RxBim <see cref="PluginResult"/> to native Revit result.
        /// </summary>
        /// <param name="commandResult">RxBim plugin result.</param>
        public static Autodesk.Revit.UI.Result MapResultToRevitResult(this PluginResult commandResult)
        {
            return commandResult.Result switch
            {
                Result.Succeeded => Autodesk.Revit.UI.Result.Succeeded,
                Result.Cancelled => Autodesk.Revit.UI.Result.Cancelled,
                Result.Failed => Autodesk.Revit.UI.Result.Failed,
                _ => Autodesk.Revit.UI.Result.Succeeded
            };
        }
    }
}