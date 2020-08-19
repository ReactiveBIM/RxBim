namespace PikTools.CommandExample
{
    using Command.Api;
    using Shared;

    /// <inheritdoc />
    public class Cmd : PikToolsCommand
    {
        /// <summary>
        /// cmd
        /// </summary>
        /// <param name="service">service</param>
        public PluginResult ExecuteCommand(IMyService service)
        {
            service.Go();
            return PluginResult.Succeeded;
        }
    }
}