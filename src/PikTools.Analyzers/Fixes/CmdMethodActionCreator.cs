namespace PikTools.Analyzers.Fixes
{
    using SymbolActions;

    /// <inheritdoc />
    public class CmdMethodActionCreator : MethodReturnTypeActionCreator
    {
        /// <inheritdoc />
        public override string DiagnosticId => CmdMethodAction.DiagnosticId;
    }
}