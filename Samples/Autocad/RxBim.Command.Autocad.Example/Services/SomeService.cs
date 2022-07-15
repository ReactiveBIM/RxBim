namespace RxBim.Command.Autocad.Example.Services
{
    using Abstractions;
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using JetBrains.Annotations;

    /// <inheritdoc/>
    [UsedImplicitly]
    public class SomeService : ISomeService
    {
        /// <inheritdoc/>
        public void DoSomething()
        {
            Application.ShowAlertDialog("Something done successfully!");
        }
    }
}