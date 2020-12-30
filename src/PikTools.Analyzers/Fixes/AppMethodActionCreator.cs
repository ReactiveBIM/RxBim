namespace PikTools.Analyzers.Fixes
{
    using SymbolActions;

    /// <inheritdoc />
    public class AppMethodActionCreator : MethodReturnTypeActionCreator
    {
        /// <inheritdoc />
        public override string DiagnosticId => AppMethodAction.DiagnosticId;
    }
}