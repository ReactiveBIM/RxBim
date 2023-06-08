namespace RxBim.Nuke.Generators
{
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using static GitHubActionsSourceUtils;
    using static PropertiesSourceUtils;
    using static TargetsSourceUtils;

    /// <summary>
    /// Generates source for Build class.
    /// </summary>
    [Generator]
    public class BuildSourcesGenerator : ISourceGenerator
    {
        /// <inheritdoc />
        public void Initialize(GeneratorInitializationContext context)
        {
        }

        /// <inheritdoc />
        public void Execute(GeneratorExecutionContext context)
        {
            var build = context.Compilation.GetTypeByMetadataName("Build");
            if (build is null || !context.IsSymbolInContext(build))
                return;

            var propsSource = GetVersionPropertiesSource();
            context.AddSource("Build.Versions.Properties.g.cs", propsSource);

            if (!context.TryGetVersionNumbersFromReferencedAssembly(out var versionNumbers))
                return;

            var buildDeclaredTargets = build.GetPropertiesNames("Target");

            var targetsSource = GetVersionBuildTargetsSource(versionNumbers, buildDeclaredTargets);
            context.AddSource("Build.Versions.Targets.g.cs", targetsSource);

            var gitHubActionsAttributes = build.GetAttributes("GitHubActionsAttribute");
            if (!gitHubActionsAttributes.Any())
                return;

            foreach (var actionsAttribute in gitHubActionsAttributes)
            {
                var source = GetVersionActionsSource(actionsAttribute, versionNumbers, out var actionName);
                if (!string.IsNullOrEmpty(actionName))
                    context.AddSource($"Build.Versions.GitHubActions.{actionName}.g.cs", source);
            }
        }
    }
}