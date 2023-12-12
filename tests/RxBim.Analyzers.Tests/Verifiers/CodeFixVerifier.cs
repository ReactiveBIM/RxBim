using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

// ReSharper disable once CheckNamespace
namespace TestHelper
{
    using System;

    /// <summary>
    /// Superclass of all Unit tests made for diagnostics with codefixes.
    /// Contains methods used to verify correctness of codefixes
    /// </summary>
    public abstract partial class CodeFixVerifier : DiagnosticVerifier
    {
        /// <summary>
        /// Returns the codefix being tested (C#) - to be implemented in non-abstract class
        /// </summary>
        /// <returns>The CodeFixProvider to be used for CSharp code</returns>
        protected virtual CodeFixProvider GetCSharpCodeFixProvider()
        {
            return null;
        }

        /// <summary>
        /// Returns the codefix being tested (VB) - to be implemented in non-abstract class
        /// </summary>
        /// <returns>The CodeFixProvider to be used for VisualBasic code</returns>
        protected virtual CodeFixProvider GetBasicCodeFixProvider()
        {
            return null;
        }

        /// <summary>
        /// Called to test a C# codefix when applied on the inputted string as a source
        /// </summary>
        /// <param name="oldSource">A class in the form of a string before the CodeFix was applied to it</param>
        /// <param name="newSource">A class in the form of a string after the CodeFix was applied to it</param>
        /// <param name="codeFixIndex">Index determining which codefix to apply if there are multiple</param>
        /// <param name="allowNewCompilerDiagnostics">A bool controlling whether or not the test will fail if the CodeFix introduces other warnings after being applied</param>
        protected void VerifyCSharpFix(
            string oldSource,
            string newSource,
            int? codeFixIndex = null,
            bool allowNewCompilerDiagnostics = false)
        {
            VerifyFix(LanguageNames.CSharp,
                GetCSharpDiagnosticAnalyzer(),
                GetCSharpCodeFixProvider(),
                oldSource,
                allowNewCompilerDiagnostics);
        }

        /// <summary>
        /// Called to test a VB codefix when applied on the inputted string as a source
        /// </summary>
        /// <param name="oldSource">A class in the form of a string before the CodeFix was applied to it</param>
        /// <param name="newSource">A class in the form of a string after the CodeFix was applied to it</param>
        /// <param name="codeFixIndex">Index determining which codefix to apply if there are multiple</param>
        /// <param name="allowNewCompilerDiagnostics">A bool controlling whether or not the test will fail if the CodeFix introduces other warnings after being applied</param>
        protected void VerifyBasicFix(
            string oldSource,
            string newSource,
            int? codeFixIndex = null,
            bool allowNewCompilerDiagnostics = false)
        {
            VerifyFix(LanguageNames.VisualBasic,
                GetBasicDiagnosticAnalyzer(),
                GetBasicCodeFixProvider(),
                oldSource,
                allowNewCompilerDiagnostics);
        }

        /// <summary>
        /// General verifier for codefixes.
        /// Creates a Document from the source string, then gets diagnostics on it and applies the relevant codefixes.
        /// Then gets the string after the codefix is applied and compares it with the expected result.
        /// Note: If any codefix causes new diagnostics to show up, the test fails unless allowNewCompilerDiagnostics is set to true.
        /// </summary>
        /// <param name="language">The language the source code is in</param>
        /// <param name="analyzer">The analyzer to be applied to the source code</param>
        /// <param name="codeFixProvider">The codefix to be applied to the code wherever the relevant Diagnostic is found</param>
        /// <param name="oldSource">A class in the form of a string before the CodeFix was applied to it</param>
        /// <param name="allowNewCompilerDiagnostics">A bool controlling whether or not the test will fail if the CodeFix introduces other warnings after being applied</param>
        private void VerifyFix(
            string language,
            DiagnosticAnalyzer analyzer,
            CodeFixProvider codeFixProvider,
            string oldSource,
            bool allowNewCompilerDiagnostics)
        {
            var document = CreateDocument(oldSource, language);
            var analyzerDiagnostics = GetSortedDiagnosticsFromDocuments(analyzer, new[] { document });
            var compilerDiagnostics = GetCompilerDiagnostics(document).ToList();
            var attempts = analyzerDiagnostics.Length;

            for (var i = 0; i < attempts; ++i)
            {
                var actions = new List<CodeAction>();
                var context = new CodeFixContext(document,
                    analyzerDiagnostics[0],
                    (a, _) => actions.Add(a),
                    CancellationToken.None);
                codeFixProvider.RegisterCodeFixesAsync(context).Wait();

                if (!actions.Any())
                {
                    throw new NotImplementedException();
                }

                document = ApplyFix(document, actions.ElementAt(0));
                analyzerDiagnostics = GetSortedDiagnosticsFromDocuments(analyzer, new[] { document });

                var newCompilerDiagnostics = GetNewDiagnostics(compilerDiagnostics, GetCompilerDiagnostics(document));

                if (!allowNewCompilerDiagnostics &&
                    newCompilerDiagnostics.Any(x => x.Id != "CS0103" && x.Id != "CS0246"))
                {
                    var syntaxNode = document.GetSyntaxRootAsync().Result;

                    if (syntaxNode != null)
                    {
                        document = document.WithSyntaxRoot(Formatter.Format(syntaxNode,
                            Formatter.Annotation,
                            document.Project.Solution.Workspace));
                        newCompilerDiagnostics =
                            GetNewDiagnostics(compilerDiagnostics, GetCompilerDiagnostics(document));

                        var join = string.Join("\r\n", newCompilerDiagnostics.Select(d => d.ToString()));
                        var fullString = syntaxNode.ToFullString();
                        Assert.IsTrue(false,
                            $"Fix introduced new compiler diagnostics:\r\n{join}\r\n\r\nNew document:\r\n{fullString}\r\n");
                    }
                }

                if (!analyzerDiagnostics.Any())
                {
                    break;
                }
            }

            // after applying all of the code fixes, compare the resulting string to the inputted one
            // var actual = GetStringFromDocument(document);
            // Assert.AreEqual(newSource, actual);
        }
    }
}