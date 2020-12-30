using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TestHelper;
using PikTools.Analyzers;

namespace PikTools.Analyzers.Test
{
    using PikTools.Analyzers;

    [TestClass]
    public class UnitTest : CodeFixVerifier
    {
        [TestMethod]
        public void CommandTest()
        {
            var test = @"
namespace PikTools.CommandExample
{
    using Command.Api;
    using Shared;

    public class Cmd : PikToolsCommand
    {
    }";
            var expected = new DiagnosticResult
            {
                Id = "PikToolsAnalyzersCommand",
                Message = String.Format("Command type '{0}' not contains \"ExecuteCommand\" method", "Cmd"),
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 7, 18)
                    }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void CommandMethodTest()
        {
            var test = @"
namespace PikTools.CommandExample
{
    using Command.Api;
    using Shared;

    public class Cmd : PikToolsCommand
    {
        public void ExecuteCommand(IMyService service)
        {
        }
    }";
            var expected = new DiagnosticResult
            {
                Id = "PikToolsAnalyzersCommandReturnType",
                Message = String.Format("Method '{0}' not returns \"PluginResult\" type", "ExecuteCommand"),
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 9, 21)
                    }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void AppTest()
        {
            var test = @"
namespace PikTools.CommandExample
{
    using Command.Api;
    using Shared;

    public class App : PikToolsApplication
    {
    }";
            var expected = new DiagnosticResult
            {
                Id = "PikToolsAnalyzersAppStart",
                Message = String.Format("App type '{0}' not contains \"Start\" method", "App"),
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 7, 18)
                    }
            };

            var expected2 = new DiagnosticResult
            {
                Id = "PikToolsAnalyzersAppShutdown",
                Message = String.Format("App type '{0}' not contains \"Shutdown\" method", "App"),
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 7, 18)
                    }
            };

            VerifyCSharpDiagnostic(test, expected, expected2);
        }

        [TestMethod]
        public void AppMethodsTest()
        {
            var test = @"
namespace PikTools.CommandExample
{
    using Command.Api;
    using Shared;

    public class App : PikToolsApplication
    {
        public void Start()
        {
        }

        public void Shutdown()
        {
        }
    }";
            var expected = new DiagnosticResult
            {
                Id = "PikToolsAnalyzersAppMethodReturType",
                Message = String.Format("Method '{0}' not returns \"PluginResult\" type", "Start"),
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 9, 21)
                    }
            };

            var expected2 = new DiagnosticResult
            {
                Id = "PikToolsAnalyzersAppMethodReturType",
                Message = String.Format("Method '{0}' not returns \"PluginResult\" type", "Shutdown"),
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 13, 21)
                    }
            };

            VerifyCSharpDiagnostic(test, expected, expected2);
        }

        [TestMethod]
        public void FixAddingCommandMethod()
        {
            var test = @"namespace PikTools.CommandExample
            {
                using Command.Api;
                using Shared;

        public class Cmd : PikToolsCommand
        {
        }
}";

            var fixtest = @"namespace PikTools.CommandExample
{
    using Command.Api;
    using Shared;

    public class Cmd : PikToolsCommand
    {
        public PluginResult ExecuteCommand()
        {
            return PluginResult.Succeeded;
        }
    }
}";
            VerifyCSharpFix(test, fixtest);
        }

        [TestMethod]
        public void FixCommandMethodReturnType()
        {
            var test = @"namespace PikTools.CommandExample
{
    using Command.Api;
    using Shared;

    public class Cmd : PikToolsCommand
    {
        public void ExecuteCommand()
        {
            return PluginResult.Succeeded;
        }
    }
}";

            var fixtest = @"namespace PikTools.CommandExample
{
    using Command.Api;
    using Shared;

    public class Cmd : PikToolsCommand
    {
        public PluginResult ExecuteCommand()
        {
            return PluginResult.Succeeded;
        }
    }
}";
            VerifyCSharpFix(test, fixtest);
        }
        
        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new PikToolsAnalyzersCodeFixProvider();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new PikToolsAnalyzersAnalyzer();
        }
    }
}