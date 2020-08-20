namespace PikTools.Analyzers
{
    using Microsoft.CodeAnalysis;

    public static class MethodRules
    {
        private static readonly LocalizableString Title = "Method returns \"PluginResult\" method.";

        private static readonly LocalizableString MessageFormat =
            "Method '{0}' not returns \"PluginResult\" type";

        private static readonly LocalizableString Description =
            "\"ExecuteCommand\" method should return \"PluginResult\" type.";

        internal static readonly DiagnosticDescriptor MethodRule = new DiagnosticDescriptor(Constants.DiagnosticId,
            Title,
            MessageFormat,
            Constants.Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);
    }
}