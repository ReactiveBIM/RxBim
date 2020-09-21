namespace PikTools.Di.Tests.TestObjects
{
    using TestDependencies;

    public class ObjectWithDependencies
    {
        public int Execute(IBaseService baseService, IPluginService pluginService)
        {
            return pluginService.GetInt();
        }
    }
}