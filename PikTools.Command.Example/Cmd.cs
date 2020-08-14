namespace PikTools.CommandExample
{
    using Command.Api;

    /// <inheritdoc />
    public class Cmd : PikToolsCommand
    {
        /// <summary>
        /// cmd
        /// </summary>
        /// <param name="service">service</param>
        public CommandResult ExecuteCommand(IMyService service)
        {
            service.Go();
            return CommandResult.Succeeded;
        }
    }
}