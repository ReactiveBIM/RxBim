namespace RxBim.Analyzers.Fixes
{
    using SymbolActions;

    /// <inheritdoc />
    public class AppStartActionCreator : AddMethodActionCreator
    {
        /// <inheritdoc />
        public override string DiagnosticId => AppAction.StartDiagnosticId;

        /// <inheritdoc />
        protected override string Title => $"Create method {MethodName}";

        /// <inheritdoc />
        protected override string MethodName => Constants.Start;
    }
}