#pragma warning disable CS1591, SA1600
namespace RxBim.MsiBuilder
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Di;
    using Nuke;
    using WixSharp;
    using WixSharp.CommonTasks;
    using File = WixSharp.File;

    public class WixInstaller
    {
        public Project CreateProject(BuildOptions buildOptions)
        {
            var projectName = buildOptions.ProjectName;
            var productProjectName = buildOptions.ProductProjectName;
            var installDir = buildOptions.InstallDir;

            var sourceDir = buildOptions.SourceDir;
            var version = buildOptions.Version is null ? new Version() : new Version(buildOptions.Version);
            var environmentRegKey =
                @$"{EnvironmentRegistryConstants.RxBimEnvironmentRegPath}\{{{buildOptions.PackageGuid}}}";

            var project = new ManagedProject(
                productProjectName,
                new Dir(installDir,
                    FillEntities(null, new[] { buildOptions.BundleDir }, false)
                        .Concat(new[]
                        {
                            new Dir(projectName, FillEntities(null, new[] { sourceDir }).ToArray())
                        }).ToArray()))
            {
                Description = buildOptions.Description,
                Package =
                {
                    AttributesDefinition =
                        $"AdminImage=yes; Comments={buildOptions.Comments}; Description={buildOptions.Description ?? buildOptions.ProjectName}"
                },
                GUID = buildOptions.PackageGuid is null ? null : new Guid(buildOptions.PackageGuid),
                UpgradeCode = buildOptions.UpgradeCode is null ? null : new Guid(buildOptions.UpgradeCode),
                MajorUpgradeStrategy = new MajorUpgradeStrategy
                {
                    UpgradeVersions = VersionRange.ThisAndOlder,
                    PreventDowngradingVersions = VersionRange.NewerThanThis,
                    NewerProductInstalledErrorMessage = "Newer version already installed",
                    RemoveExistingProductAfter = Step.InstallInitialize
                },
                Version = version,
                UI = WUI.WixUI_ProgressOnly,
                InstallScope = InstallScope.perUser,
                ControlPanelInfo = { Manufacturer = "ReactiveBIM" },
                Encoding = Encoding.UTF8,
                Codepage = "1251",
                OutDir = buildOptions.OutDir,
                OutFileName = $"{buildOptions.OutFileName}_{version}",
                RegValues = new[]
                {
                    new RegValue(
                        RegistryHive.CurrentUser,
                        environmentRegKey,
                        EnvironmentRegistryConstants.EnvironmentRegKeyName,
                        buildOptions.Environment),
                    new RegValue(
                        RegistryHive.CurrentUser,
                        environmentRegKey,
                        EnvironmentRegistryConstants.PluginNameRegKeyName,
                        buildOptions.ProductProjectName)
                }
            };

            project.AddAction(
                new ManagedAction(
                    CustomActions.InstallFonts,
                    GetType().Assembly.Location,
                    Return.ignore,
                    When.After,
                    Step.InstallFinalize,
                    Condition.Always));

            var attributesDefinition = $"AdminImage=yes;";
            if (!string.IsNullOrEmpty(buildOptions.Comments))
            {
                attributesDefinition += $"Comments={buildOptions.Comments}; ";
            }

            if (!string.IsNullOrEmpty(buildOptions.Description))
            {
                attributesDefinition += $"Description={buildOptions.Description}; ";
            }

            project.Package.AttributesDefinition = attributesDefinition;

            return project;
        }

        private IEnumerable<WixEntity> FillEntities(
            Dir? parent,
            IEnumerable<string?> paths,
            bool withSubDirectories = true)
        {
            var added = new List<string>();
            var files = new List<File>();
            var dirs = new List<Dir>();

            foreach (var path in paths)
            {
                if (path is null)
                    continue;

                foreach (var file in Directory.EnumerateFiles(path, "*"))
                {
                    var info = new FileInfo(file);
                    if (added.Contains(info.Name))
                        continue;
                    added.Add(info.Name);

                    files.Add(new File(file)
                    {
                        OverwriteOnInstall = true
                    });
                }

                if (withSubDirectories)
                {
                    var childDirectories = Directory.EnumerateDirectories(path);

                    foreach (var dirPath in childDirectories)
                    {
                        var dir = new Dir(Path.GetFileName(dirPath));
                        FillEntities(dir, new[] { dirPath });
                        dirs.Add(dir);
                    }
                }

                if (parent != null)
                {
                    parent.Files = files.ToArray();
                    parent.Dirs = dirs.ToArray();
                }
            }

            var result = new List<WixEntity>();

            result.AddRange(files);
            result.AddRange(dirs);

            return result;
        }
    }
}