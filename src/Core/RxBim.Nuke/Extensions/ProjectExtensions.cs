namespace RxBim.Nuke.Extensions
{
    extern alias nc;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using Builds;
    using Models;
    using MsiBuilder;
    using nc::Nuke.Common.IO;
    using nc::Nuke.Common.ProjectModel;
    using nc::Nuke.Common.Tooling;
    using nc::Nuke.Common.Tools.DotNet;
    using nc::Nuke.Common.Tools.Git;
    using nc::Nuke.Common.Utilities;
    using Serilog;
    using static Constants;
    using static Helpers.AssemblyScanner;
    using static nc::Nuke.Common.Tools.DotNet.DotNetTasks;

    /// <summary>
    /// Project extensions.
    /// </summary>
    public static class ProjectExtensions
    {
        /// <summary>
        /// Gets setup options.
        /// </summary>
        /// <param name="project">Project.</param>
        /// <param name="installDir">Install directory.</param>
        /// <param name="sourceDir">Source build directory.</param>
        /// <param name="configuration">Configuration.</param>
        public static Options GetSetupOptions(
            this Project project,
            string installDir,
            string sourceDir,
            string configuration)
        {
            var productVersion = project.GetProperty(nameof(Options.ProductVersion));
            if (string.IsNullOrWhiteSpace(productVersion)
                && configuration.Equals(Configuration.Release))
            {
                throw new ArgumentException(
                    $"Project {project.Name} should contain '{nameof(Options.ProductVersion)}' property with product version value!");
            }

            var msiFilePrefix = project.GetProperty(nameof(Options.MsiFilePrefix));
            var outputFileName = $"{msiFilePrefix}{project.Name}";

            if (!string.IsNullOrWhiteSpace(productVersion))
                outputFileName += $"_{productVersion}";

            var version = project.GetProperty(nameof(Options.Version)) ??
                          throw new ArgumentException(
                              $"Project {project.Name} should contain '{nameof(Options.Version)}' property with valid version value!");

            if (configuration == Configuration.Debug)
            {
                var unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                version += $".{unixTimestamp}";
            }

            var options = new Options
            {
                Comments = project.GetProperty(nameof(Options.Comments)),
                Description = project.GetProperty(nameof(Options.Description)),
                Version = version,
                ProductVersion = productVersion,
                BundleDir = sourceDir,
                InstallDir = installDir,
                ManifestDir = sourceDir,
                OutDir = project.Solution.Directory / "out",
                PackageGuid = project.GetProperty(nameof(Options.PackageGuid)) ??
                              throw new ArgumentException(
                                  $"Project {project.Name} should contain '{nameof(Options.PackageGuid)}' property with valid guid value!"),
                UpgradeCode = project.GetProperty(nameof(Options.UpgradeCode)) ??
                              throw new ArgumentException(
                                  $"Project {project.Name} should contain '{nameof(Options.UpgradeCode)}' property with valid guid value!"),
                ProjectName = project.Name,
                ProductProjectName = outputFileName,
                SourceDir = Path.Combine(sourceDir, "bin"),
                OutFileName = outputFileName,
                AddAllAppToManifest = Convert.ToBoolean(project.GetProperty(nameof(Options.AddAllAppToManifest))),
                ProjectsAddingToManifest = project.GetProperty(nameof(Options.ProjectsAddingToManifest))
                    ?.Split(',', StringSplitOptions.RemoveEmptyEntries),
                SetupIcon = project.GetProperty(nameof(Options.SetupIcon)),
                UninstallIcon = project.GetProperty(nameof(Options.UninstallIcon))
            };
            return options;
        }

        /// <summary>
        /// Builds Msi.
        /// </summary>
        /// <param name="project">Project.</param>
        /// <param name="toolPath">Build MSI tool path.</param>
        /// <param name="options">Options.</param>
        public static void BuildMsiWithTool(
            this Project project,
            string toolPath,
            Options options)
        {
            var p = ProcessTasks.StartProcess(
                toolPath,
                options.ToString(),
                project.Solution.Directory / "out");

            p.WaitForExit();

            if (p.ExitCode != 0)
            {
                throw new ApplicationException("Building MSI package failed!!!");
            }
        }

        /// <summary>
        /// DotNet builds project and returns build path.
        /// </summary>
        /// <param name="project">Project.</param>
        /// <param name="config">Configuration.</param>
        public static AbsolutePath BuildProject(this Project project, string config)
        {
            DotNetBuild(settings => settings
                .SetConfiguration(config)
                .SetProjectFile(project));

            var binPath = project.GetTargetPath();
            return binPath;
        }

        /// <summary>
        /// Adds properties to a project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="properties">Properties via <see cref="XElement"/> collection.</param>
        public static void AddPropertiesToProject(this Project project, IReadOnlyCollection<XElement> properties)
        {
            if (properties.Any())
            {
                var projectXml = XElement.Load(project.Path);

                projectXml.Add(new XElement("PropertyGroup", properties));
                projectXml.Save(project.Path);

                Log.Information("Properties {Properties} for {ProjectName} project added\"",
                    properties.Select(x => x.Name.ToString()).ToList().JoinComma(),
                    project.Name);

                project.CommitChanges();
            }
        }

        /// <summary>
        /// Generates a project properties for installation.
        /// </summary>
        /// <param name="project">The project.</param>
        public static IEnumerable<XElement> GenerateInstallationProperties(this Project project)
        {
            if (project.GetProperty(nameof(Options.PackageGuid)) == null)
            {
                yield return new XElement(nameof(Options.PackageGuid), Guid.NewGuid());
            }

            if (project.GetProperty(nameof(Options.UpgradeCode)) == null)
            {
                yield return new XElement(nameof(Options.UpgradeCode), Guid.NewGuid());
            }
        }

        /// <summary>
        /// Gets the <see cref="AssemblyType"/> collection from a project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="output">Output path.</param>
        /// <param name="options">Options.</param>
        public static List<AssemblyType> GetAssemblyTypes(
            this Project project,
            string output,
            Options options)
        {
            var file = Path.Combine(output, $"{project.Name}.dll");

            var types = GetAssemblyTypes(file, new[] { RxBimCommand, RxBimApplication });

            var additionalFiles = new List<string>();
            if (options.AddAllAppToManifest)
            {
                // Adds all dependency assemblies from the output path
                additionalFiles = Directory.GetFiles(output, "*.dll").ToList();
                additionalFiles.Remove(file);
            }
            else if (options.ProjectsAddingToManifest != null
                     && options.ProjectsAddingToManifest.Any())
            {
                // Adds additional applications from the specified options
                additionalFiles = options.ProjectsAddingToManifest
                    .Select(p => Path.Combine(output, $"{p.Trim()}.dll"))
                    .ToList();
                if (additionalFiles.Any(f => !File.Exists(f)))
                {
                    throw new FileNotFoundException(
                        $"Assembly not found from property {nameof(Options.ProjectsAddingToManifest)}");
                }
            }

            foreach (var f in additionalFiles)
            {
                types.AddRange(GetAssemblyTypes(f, new[] { RxBimApplication }));
            }

            return types;
        }

        /// <summary>
        /// Gets the target project directory path.
        /// </summary>
        /// <param name="project">The project.</param>
        public static AbsolutePath GetTargetDir(this Project project)
        {
            var targetFx = project.GetTargetFramework(out var multiple);

            var targetDir = project.Directory / project.GetProperty("OutputPath");

            if (multiple)
                targetDir /= targetFx;

            return targetDir;
        }

        /// <summary>
        /// Gets a project assembly path.
        /// </summary>
        /// <param name="project">The project.</param>
        public static AbsolutePath GetTargetPath(this Project project)
        {
            return project.GetTargetDir() / $"{project.GetProperty("AssemblyName")}.dll";
        }

        /// <summary>
        /// Maps the <see cref="Project"/> to the <see cref="ApplicationPackage"/>.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="components">Components.</param>
        public static ApplicationPackage ToApplicationPackage(this Project project, List<Components> components)
        {
            return new ApplicationPackage
            {
                Description = project.GetProperty(nameof(ApplicationPackage.Description)) ?? string.Empty,
                Name = project.Name,
                AppVersion = project.GetProperty("Version"),
                FriendlyVersion = project.GetProperty("Version"),
                ProductCode = project.GetProperty("PackageGuid"),
                UpgradeCode = project.GetProperty("UpgradeCode"),
                Components = components
            };
        }

        /// <summary>
        /// Commits changes to GIT.
        /// </summary>
        /// <param name="project">The project.</param>
        private static void CommitChanges(this Project project)
        {
            var commit = ConsoleUtility.PromptForChoice("Commit changes?", ("Yes", "Yes"), ("No", "No"));

            if (commit switch
                {
                    "Yes" => true,
                    "No" => false,
                    _ => false
                })
            {
                GitTasks.Git($"add \"{project.Path}\"", project.Solution.Directory);
                GitTasks.Git(
                    $"commit -m \"Generated properties for {project.Name} project\"",
                    project.Solution.Directory);
            }
        }

        /// <summary>
        /// Gets target framework name.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="multiple">True if specified TargetFrameworks property.</param>
        private static string GetTargetFramework(this Project project, out bool multiple)
        {
            var fxNameSingle = project.GetProperty("TargetFramework");
            if (!string.IsNullOrWhiteSpace(fxNameSingle))
            {
                multiple = false;
                return string.Empty;
            }

            var fxNameFirst = project.GetTargetFrameworks()?.FirstOrDefault();
            if (string.IsNullOrEmpty(fxNameFirst))
                throw new InvalidOperationException($"Can't find target framework for project {project.Name}");

            multiple = true;
            return fxNameFirst;
        }

        private static List<AssemblyType> GetAssemblyTypes(
            string file,
            string[] typeNames)
        {
            return Scan(file)
                .Where(x => typeNames.Contains(x.BaseTypeName))
                .ToList();
        }
    }
}