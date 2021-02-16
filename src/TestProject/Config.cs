namespace TestProject
{
    using PikTools.Di;
    using PikTools.Shared.FmHelpers;
    using PikTools.Shared.RevitExtensions;
    using PikTools.Shared.Ui;

    /// <inheritdoc />
    public class Config : ICommandConfiguration
    {
        /// <inheritdoc />
        public void Configure(IContainer container)
        {
            container.AddUi();
            container.AddRevitHelpers();
            container.AddFmHelpers();

#if RELEASE
            container.AddLogs();
#endif
        }
    }
}