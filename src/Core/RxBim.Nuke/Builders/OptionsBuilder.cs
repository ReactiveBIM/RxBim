namespace RxBim.Nuke.Builders;

extern alias nc;

using System;
using System.IO;
using Builds;
using JetBrains.Annotations;
using nc::Nuke.Common.ProjectModel;

/// <summary>
/// Builder for <see cref="Options"/>.
/// </summary>
[PublicAPI]
public class OptionsBuilder
{
    /// <summary>
    /// <see cref="Options"/>.
    /// </summary>
    protected Options Options { get; set; } = new();

    /// <summary>
    /// Builds <see cref="Options"/>.
    /// </summary>
    public Options Build()
    {
        GetOptionsModifyAction()?.Invoke(Options);
        return Options;
    }

    /// <summary>
    /// Returns action for <see cref="Options"/> modification.
    /// </summary>
    public virtual Action<Options>? GetOptionsModifyAction() => null;

    /// <summary>
    /// Adds default settings.
    /// </summary>
    /// <param name="project">Selected Project.</param>
    public virtual OptionsBuilder AddDefaultSettings(Project project)
    {
        Options = new Options
        {
            Comments = project.GetProperty(nameof(Options.Comments)),
            Description = project.GetProperty(nameof(Options.Description)),
            OutDir = project.Solution.Directory / "out",
            PackageGuid = project.GetProperty(nameof(Options.PackageGuid)) ??
                          throw new ArgumentException(
                              $"Project {project.Name} should contain '{nameof(Options.PackageGuid)}' property with valid guid value!"),
            UpgradeCode = project.GetProperty(nameof(Options.UpgradeCode)) ??
                          throw new ArgumentException(
                              $"Project {project.Name} should contain '{nameof(Options.UpgradeCode)}' property with valid guid value!"),
            ProjectName = project.Name,
            AddAllAppToManifest = Convert.ToBoolean(project.GetProperty(nameof(Options.AddAllAppToManifest))),
            ProjectsAddingToManifest = project.GetProperty(nameof(Options.ProjectsAddingToManifest))
                ?.Split(',', StringSplitOptions.RemoveEmptyEntries),
            SetupIcon = project.GetProperty(nameof(Options.SetupIcon)),
            UninstallIcon = project.GetProperty(nameof(Options.UninstallIcon))
        };

        return this;
    }

    /// <summary>
    /// Adds directory settings.
    /// </summary>
    /// <param name="installDirectory">Install directory.</param>
    /// <param name="sourceDirectory">Source build directory.</param>
    public virtual OptionsBuilder AddDirectorySettings(string installDirectory, string sourceDirectory)
    {
        Options.InstallDir = installDirectory;
        Options.BundleDir = sourceDirectory;
        Options.ManifestDir = sourceDirectory;
        Options.SourceDir = Path.Combine(sourceDirectory, "bin");
        return this;
    }

    /// <summary>
    /// Adds product version.
    /// </summary>
    /// <param name="project">Selected Project.</param>
    /// <param name="configuration">Configuration.</param>
    public virtual OptionsBuilder AddProductVersion(Project project, string configuration)
    {
        var productVersion = project.GetProperty(nameof(Options.ProductVersion));
        if (string.IsNullOrWhiteSpace(productVersion)
            && configuration.Equals(Configuration.Release))
        {
            throw new ArgumentException(
                $"Project {project.Name} should contain '{nameof(Options.ProductVersion)}' property with product version value!");
        }

        var msiFilePrefix = project.GetProperty(nameof(Options.InstallFilePrefix));
        var outputFileName = $"{msiFilePrefix}{project.Name}";

        if (!string.IsNullOrWhiteSpace(productVersion))
            outputFileName += $"_{productVersion}";

        Options.ProductVersion = productVersion;
        Options.OutFileName = outputFileName;
        return this;
    }

    /// <summary>
    /// Adds environment variable.
    /// </summary>
    /// <param name="environment">Environment variable.</param>
    public virtual OptionsBuilder AddEnvironment(string environment)
    {
        Options.Environment = environment;
        return this;
    }

    /// <summary>
    /// Adds version.
    /// </summary>
    /// <param name="project">Selected Project.</param>
    public virtual OptionsBuilder AddVersion(Project project)
    {
        Options.Version = project.GetProperty(nameof(Options.Version)) ??
                          throw new ArgumentException(
                              $"Project {project.Name} should contain '{nameof(Options.Version)}'" +
                              " property with valid version value!");
        return this;
    }

    /// <summary>
    /// Adds timestamp revision version.
    /// </summary>
    public virtual OptionsBuilder AddTimestampRevisionVersion()
    {
        if (Options.Version != null && Options.Version.Split(".").Length <= 3)
        {
            var unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            Options.Version += $".{unixTimestamp}";
        }

        return this;
    }
}