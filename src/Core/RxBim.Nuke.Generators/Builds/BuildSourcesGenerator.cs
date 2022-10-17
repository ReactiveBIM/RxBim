namespace RxBim.Nuke.Generators
{
    using System.Linq;
    using Microsoft.CodeAnalysis;

    /// <summary>
    /// Generates source for Build class.
    /// </summary>
    [Generator]
    public class BuildSourcesGenerator : ISourceGenerator
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
            if (build is null || !context.IsSymbolInContext(build))
                return;

            var propsSource = PropsSourceUtils.GetVersionPropertiesSource();
            context.AddSource("Build.Versions.Properties.g.cs", propsSource);

            if (!context.TryGetVersionNumbersFromExternalAssembly(out var versionNumbers))
                return;

            var buildDeclaredTargets = build.GetPropertiesNames("Target");

            var targetsSource = TargetsSourceUtils.GetVersionBuildTargetsSource(versionNumbers, buildDeclaredTargets);
            context.AddSource("Build.Versions.Targets.g.cs", targetsSource);

            var gitHubActionsAttributes = build.GetAttributes("GitHubActionsAttribute");
            if (!gitHubActionsAttributes.Any())
                return;

            foreach (var actionsAttribute in gitHubActionsAttributes)
            {
                var source =
                    ActionsSourceUtils.GetVersionActionsSource(actionsAttribute, versionNumbers, out var actionName);
                if (!string.IsNullOrEmpty(actionName))
                    context.AddSource($"Build.Versions.Actions.{actionName}.g.cs", source);
            }
        }
    }
}