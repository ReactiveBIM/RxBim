using PikTools.Di;
using PikTools.Shared.Ui;

namespace PikTools.WpfStyles.Example
{
    public class DiConfig : DiConfigurator<ICommandConfiguration>
    {
        protected override void ConfigureBaseDependencies()
        {
            Container.AddUi();
        }
    }
}
