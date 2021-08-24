namespace RxBim.Analyzers.Fixes
{
    using SymbolActions;

    /// <inheritdoc />
    public class AppShutdownActionCreator : AddMethodActionCreator
    {
        /// <inheritdoc />
        public override string DiagnosticId => AppAction.ShutdownDiagnosticId;

        /// <inheritdoc />
        protected override string Title => $"Create method {MethodName}";

        /// <inheritdoc />
        protected override string MethodName => Constants.Shutdown;
    }
}