namespace PikTools.LoggedCommand.Example
{
    using Di;
    using Logs;

    public class Config : ICommandConfiguration
    {
        public void Configure(IContainer container)
        {
            container.AddLogs();
        }
    }
}