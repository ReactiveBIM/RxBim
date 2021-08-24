namespace RxBim.Analyzers.Fixes
{
    using SymbolActions;

    /// <inheritdoc />
    public class CmdActionCreator : AddMethodActionCreator
    {
        /// <inheritdoc />
        public override string DiagnosticId => CmdAction.DiagnosticId;

        /// <inheritdoc />
        protected override string Title => $"Create method {MethodName}";

        /// <inheritdoc />
        protected override string MethodName => "ExecuteCommand";
    }
}