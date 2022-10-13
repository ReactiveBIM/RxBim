﻿namespace RxBim.Nuke.Generators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;

    /// <summary>
    /// Generates AppVersionNumber values.
    /// </summary>
    [Generator]
    public class VersionsSourcesGenerator : ISourceGenerator
    {
        /// <inheritdoc />
        public void Initialize(GeneratorInitializationContext context)
        {
// #if DEBUG
//             Debugger.Launch();
// #endif
        }

        /// <inheritdoc />
        public void Execute(GeneratorExecutionContext context)
        {
            if (!context.TryGetVersionNumbers(out var versionNumbers, true))
                return;

            var verNumSource = GetVersionNumbersSource(versionNumbers);
            context.AddSource("AppVersionNumber.g.cs", verNumSource);

            var targetsSource = GetVersionBuildTargetsSource(versionNumbers);
            context.AddSource("IVersionBuild.g.cs", targetsSource);
        }

        private static string GetVersionNumbersSource(IEnumerable<string> versionNumbers)
        {
            var values = string.Join(Environment.NewLine,
                versionNumbers.Select(x =>
                    $@"        public static AppVersionNumber Version{x} = new() {{ Value = ""{x}"" }};"));

            return $@"// <auto-generated/>
#pragma warning disable CS1591, SA1401, SA1600, SA1601, CA2211

namespace RxBim.Nuke.Versions

{{
    public partial class AppVersionNumber
    {{
{values}
    }}
}}";
        }

        private static string GetVersionBuildTargetsSource(IEnumerable<string> versionNumbers)
        {
            var values = string.Join(Environment.NewLine,
                versionNumbers.Select(x =>
                    $@"        Target SetupEnv{x} => _ => _.Executes(() => this.SetupEnvironment(""{x}""));"));

            return $@"// <auto-generated/>
#pragma warning disable CS1591, SA1205, SA1600

namespace RxBim.Nuke.Versions
{{
    using global::Nuke.Common;

    partial interface IVersionBuild
    {{
{values}
    }}
}}";
        }
    }
}