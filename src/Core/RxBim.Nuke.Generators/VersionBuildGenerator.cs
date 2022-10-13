﻿namespace RxBim.Nuke.Generators
{
    // using System.Diagnostics;
    using Microsoft.CodeAnalysis;

    /// <summary>
    /// Generates source for Build class.
    /// </summary>
    [Generator]
    public class VersionBuildGenerator : ISourceGenerator
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
            var build = context.Compilation.GetTypeByMetadataName("Build");
            if (build is null || !context.CheckAssembly(build))
                return;

            var source = GetSource();

            context.AddSource("Build.g.cs", source);
        }

        private static string GetSource()
        {
            var source = @$"// <auto-generated>
using Nuke.Common;
using RxBim.Nuke.Versions;

/// <summary>
/// Build class.
/// </summary>
partial class Build : IVersionBuild
{{
    /// <inheritdoc />
    [Parameter]
    public AppVersion AppVersion {{ get; set; }}

    /// <inheritdoc />
    [Parameter]
    public AppVersionNumber AppVersionNumber {{ get; set; }}
}}";
            return source;
        }
    }
}