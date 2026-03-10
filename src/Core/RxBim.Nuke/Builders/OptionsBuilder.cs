namespace RxBim.Nuke.Builders;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Builds;
using global::Nuke.Common.ProjectModel;
using JetBrains.Annotations;

/// <summary>
/// Builder for <see cref="BuildOptions"/>.
/// </summary>
[PublicAPI]
public class OptionsBuilder
{
    /// <summary>
    /// Queue for <see cref="BuildOptions"/> modification.
    /// </summary>
    protected Queue<Action<BuildOptions>> OptionsModifyQueue { get; set; } = new();

    /// <summary>
    /// Builds <see cref="BuildOptions"/>.
    /// </summary>
    public BuildOptions Build()
    {
        var resultOptions = new BuildOptions();
        while (OptionsModifyQueue.Any())
        {
            OptionsModifyQueue.Dequeue().Invoke(resultOptions);
        }

        return resultOptions;
    }

    /// <summary>
    /// Adds action in <see cref="BuildOptions"/> build queue.
    /// </summary>
    /// <param name="optionsModification">Custom action for <see cref="BuildOptions"/> modification.</param>
    public OptionsBuilder AddOptionsModifyAction(Action<BuildOptions> optionsModification)
    {
        OptionsModifyQueue.Enqueue(optionsModification);
        return this;
    }

    /// <summary>
    /// Sets default settings.
    /// </summary>
    /// <param name="project">Selected Project.</param>
    public virtual OptionsBuilder SetDefaultSettings(Project project)
    {
        OptionsModifyQueue.Enqueue(opts =>
        {
            opts.Comments = project.GetProperty(nameof(BuildOptions.Comments));
            opts.Description = project.GetProperty(nameof(BuildOptions.Description));
            opts.OutDir = project.Solution.Directory / "out";
            opts.PackageGuid = project.GetProperty(nameof(BuildOptions.PackageGuid)) ??
                               throw new ArgumentException(
                                   $"Project {project.Name} should contain '{nameof(BuildOptions.PackageGuid)}' property with valid guid value!");
            opts.UpgradeCode = project.GetProperty(nameof(BuildOptions.UpgradeCode)) ??
                               throw new ArgumentException(
                                   $"Project {project.Name} should contain '{nameof(BuildOptions.UpgradeCode)}' property with valid guid value!");
            opts.ProjectName = project.Name;
            opts.AddAllAppToManifest = Convert.ToBoolean(project.GetProperty(nameof(BuildOptions.AddAllAppToManifest)));
            opts.ProjectsAddingToManifest = project.GetProperty(nameof(BuildOptions.ProjectsAddingToManifest))
                ?.Split(',', StringSplitOptions.RemoveEmptyEntries);
            opts.SetupIcon = project.GetProperty(nameof(BuildOptions.SetupIcon));
            opts.UninstallIcon = project.GetProperty(nameof(BuildOptions.UninstallIcon));
        });

        return this;
    }

    /// <summary>
    /// Sets directory settings.
    /// </summary>
    /// <param name="installDirectory">Install directory.</param>
    /// <param name="sourceDirectory">Source build directory.</param>
    public virtual OptionsBuilder SetDirectorySettings(string installDirectory, string sourceDirectory)
    {
        OptionsModifyQueue.Enqueue(opts =>
        {
            opts.InstallDir = installDirectory;
            opts.BundleDir = sourceDirectory;
            opts.ManifestDir = sourceDirectory;
            opts.SourceDir = Path.Combine(sourceDirectory, "bin");
        });

        return this;
    }

    /// <summary>
    /// Sets product version.
    /// </summary>
    /// <param name="project">Selected Project.</param>
    /// <param name="configuration">Configuration.</param>
    public virtual OptionsBuilder SetProductVersion(Project project, string configuration)
    {
        var productVersion = project.GetProperty(nameof(BuildOptions.ProductVersion));
        if (string.IsNullOrWhiteSpace(productVersion)
            && configuration.Equals(Configuration.Release))
        {
            throw new ArgumentException(
                $"Project {project.Name} should contain '{nameof(BuildOptions.ProductVersion)}' property with product version value!");
        }

        var msiFilePrefix = project.GetProperty(nameof(BuildOptions.InstallFilePrefix));
        var outputFileName = $"{msiFilePrefix}{project.Name}";

        if (!string.IsNullOrWhiteSpace(productVersion))
            outputFileName += $"_{productVersion}";

        OptionsModifyQueue.Enqueue(opts =>
        {
            opts.ProductVersion = productVersion;
            opts.OutFileName = outputFileName;
            opts.ProductProjectName = outputFileName;
        });

        return this;
    }

    /// <summary>
    /// Sets environment variable.
    /// </summary>
    /// <param name="environment">Environment variable.</param>
    public virtual OptionsBuilder SetEnvironment(string environment)
    {
        OptionsModifyQueue.Enqueue(opts =>
        {
            opts.Environment = environment;
        });

        return this;
    }

    /// <summary>
    /// Sets version.
    /// </summary>
    /// <param name="project">Selected Project.</param>
    public virtual OptionsBuilder SetVersion(Project project)
    {
        OptionsModifyQueue.Enqueue(opts =>
        {
            opts.Version = project.GetProperty(nameof(BuildOptions.Version)) ??
                           throw new ArgumentException(
                               $"Project {project.Name} should contain '{nameof(BuildOptions.Version)}'" +
                               " property with valid version value!");
        });

        return this;
    }

    /// <summary>
    /// Sets timestamp revision version.
    /// </summary>
    public virtual OptionsBuilder SetTimestampRevisionVersion()
    {
        OptionsModifyQueue.Enqueue(opts =>
        {
            if (opts.Version != null && opts.Version.Split(".").Length <= 3)
            {
                var unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                opts.Version += $".{unixTimestamp}";
            }
        });

        return this;
    }
}