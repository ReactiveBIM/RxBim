namespace PikTools.LoggedCommand.Example
{
    using Di;
    using Logs;
    using SimpleInjector;

    public class Config : ICommandConfiguration
    {
        public void Configure(Container container)
        {
            container.AddLogs();
        }
    }
}