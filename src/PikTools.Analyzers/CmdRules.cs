namespace PikTools.Analyzers
{
    using Microsoft.CodeAnalysis;

    public static class CmdRules
    {
        private static readonly LocalizableString Title = "Command type contains \"ExecuteCommand\" method.";

        private static readonly LocalizableString MessageFormat =
            "Command type '{0}' not contains \"ExecuteCommand\" method";

        private static readonly LocalizableString
            Description = "Command type should contain \"ExecuteCommand\" method.";

        internal static readonly DiagnosticDescriptor CommandRule = new DiagnosticDescriptor(Constants.DiagnosticId, Title,
            MessageFormat,
            Constants.Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

        
    }
}