namespace PikTools.Analyzers
{
    using Microsoft.CodeAnalysis;

    public static class AppRules
    {
        private static readonly LocalizableString StartMethodTitle = "App type contains \"Start\" method.";

        private static readonly LocalizableString StartMethodMessageFormat =
            "App type '{0}' not contains \"Start\" method";

        private static readonly LocalizableString StartMethodDescription = "App type should contain \"Start\" method.";

        internal static readonly DiagnosticDescriptor AppStartMethodRule = new DiagnosticDescriptor(Constants.DiagnosticId,
            StartMethodTitle,
            StartMethodMessageFormat,
            Constants.Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: StartMethodDescription);

        private static readonly LocalizableString ShutdownMethodTitle = "App type contains \"Shutdown\" method.";

        private static readonly LocalizableString ShutdownMethodMessageFormat =
            "App type '{0}' not contains \"Shutdown\" method";

        private static readonly LocalizableString ShutdownMethodDescription =
            "App type should contain \"Shutdown\" method.";

        internal static readonly DiagnosticDescriptor AppShutdownMethodRule = new DiagnosticDescriptor(Constants.DiagnosticId,
            ShutdownMethodTitle,
            ShutdownMethodMessageFormat,
            Constants.Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: ShutdownMethodDescription);
    }
}