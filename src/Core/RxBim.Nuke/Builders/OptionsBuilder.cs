namespace RxBim.Nuke.Builders;

extern alias nc;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    /// Queue for <see cref="Options"/> modification.
    /// </summary>
    protected Queue<Action<Options>> OptionsModifyQueue { get; set; } = new();

    /// <summary>
    /// Builds <see cref="Options"/>.
    /// </summary>
    public Options Build()
    {
        var customModification = GetOptionsModifyAction();
        if (customModification != null)
            OptionsModifyQueue.Enqueue(customModification);

        var resultOptions = new Options();
        while (OptionsModifyQueue.Any())
        {
            OptionsModifyQueue.Dequeue()?.Invoke(resultOptions);
        }

        return resultOptions;
    }

    /// <summary>
    /// Adds default settings.
    /// </summary>
    /// <param name="project">Selected Project.</param>
    public virtual OptionsBuilder SetDefaultSettings(Project project)
    {
        OptionsModifyQueue.Enqueue(opts =>
        {
            opts.Comments = project.GetProperty(nameof(Options.Comments));
            opts.Description = project.GetProperty(nameof(Options.Description));
            opts.OutDir = project.Solution.Directory / "out";
            opts.PackageGuid = project.GetProperty(nameof(Options.PackageGuid)) ??
                               throw new ArgumentException(
                                   $"Project {project.Name} should contain '{nameof(Options.PackageGuid)}' property with valid guid value!");
            opts.UpgradeCode = project.GetProperty(nameof(Options.UpgradeCode)) ??
                               throw new ArgumentException(
                                   $"Project {project.Name} should contain '{nameof(Options.UpgradeCode)}' property with valid guid value!");
            opts.ProjectName = project.Name;
            opts.AddAllAppToManifest = Convert.ToBoolean(project.GetProperty(nameof(Options.AddAllAppToManifest)));
            opts.ProjectsAddingToManifest = project.GetProperty(nameof(Options.ProjectsAddingToManifest))
                ?.Split(',', StringSplitOptions.RemoveEmptyEntries);
            opts.SetupIcon = project.GetProperty(nameof(Options.SetupIcon));
            opts.UninstallIcon = project.GetProperty(nameof(Options.UninstallIcon));
        });

        return this;
    }

    /// <summary>
    /// Adds directory settings.
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
    /// Adds product version.
    /// </summary>
    /// <param name="project">Selected Project.</param>
    /// <param name="configuration">Configuration.</param>
    public virtual OptionsBuilder SetProductVersion(Project project, string configuration)
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

        OptionsModifyQueue.Enqueue(opts =>
        {
            opts.ProductVersion = productVersion;
            opts.OutFileName = outputFileName;
            opts.ProductProjectName = outputFileName;
        });

        return this;
    }

    /// <summary>
    /// Adds environment variable.
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
    /// Adds version.
    /// </summary>
    /// <param name="project">Selected Project.</param>
    public virtual OptionsBuilder SetVersion(Project project)
    {
        OptionsModifyQueue.Enqueue(opts =>
        {
            opts.Version = project.GetProperty(nameof(Options.Version)) ??
                           throw new ArgumentException(
                               $"Project {project.Name} should contain '{nameof(Options.Version)}'" +
                               " property with valid version value!");
        });

        return this;
    }

    /// <summary>
    /// Adds timestamp revision version.
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

    /// <summary>
    /// Returns action for <see cref="Options"/> modification.
    /// </summary>
    protected virtual Action<Options>? GetOptionsModifyAction() => null;
}