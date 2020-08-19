namespace PikTools.Shared
{
    /// <summary>
    /// Расширения для <see cref="PluginResult"/>
    /// </summary>
    public static class PluginResultExtensions
    {
        /// <summary>
        /// Переобразует результат в результат Revit
        /// </summary>
        /// <param name="commandResult">результат</param>
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