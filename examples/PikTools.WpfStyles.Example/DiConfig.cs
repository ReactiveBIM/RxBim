using PikTools.Di;
using PikTools.Shared.Ui;
using System;
using System.Collections.Generic;
using System.Text;

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
