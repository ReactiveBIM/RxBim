namespace PikTools.MsiBuilder
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using WixSharp;
    using WixSharp.CommonTasks;
    using File = WixSharp.File;

    public class WixInstaller
    {
        public Project CreateProject(Options options)
        {
            var projectName = options.ProjectName;
            var productProjectName = options.ProductProjectName;
            var installDir = options.InstallDir;

            var sourceDir = options.SourceDir;

            var project = new ManagedProject(
                productProjectName,
                new Dir(installDir,
                    FillEntities(null, new[] { options.BundleDir }, false)
                        .Concat(new[]
                        {
                            new Dir(projectName, FillEntities(null, new[] { sourceDir }).ToArray())
                        }).ToArray()
                )
            )
            {
                Description = options.Description,
                Package =
                {
                    AttributesDefinition =
                        $"AdminImage=yes; Comments={options.Comments}; Description={options.Description ?? options.ProjectName}"
                },
                GUID = new Guid(options.PackageGuid),
                UpgradeCode = new Guid(options.UpgradeCode),
                MajorUpgradeStrategy = new MajorUpgradeStrategy
                {
                    UpgradeVersions = VersionRange.ThisAndOlder,
                    PreventDowngradingVersions = VersionRange.NewerThanThis,
                    NewerProductInstalledErrorMessage = "Newer version already installed",
                    RemoveExistingProductAfter = Step.InstallInitialize
                },
                Version = new Version(options.Version),
                UI = WUI.WixUI_ProgressOnly,
                InstallScope = InstallScope.perUser,
                ControlPanelInfo = { Manufacturer = "PIK" },
                Encoding = Encoding.UTF8,
                Codepage = "1251",
                OutDir = options.OutDir,
                OutFileName = options.OutFileName + "_" + options.Version
            };

            project.AddAction(
                new ManagedAction(
                    CustomActions.InstallFonts, GetType().Assembly.Location, Return.ignore, When.After, Step.InstallFinalize, Condition.Always));

            var attributesDefinition = $"AdminImage=yes;";
            if (!string.IsNullOrEmpty(options.Comments))
            {
                attributesDefinition += $"Comments={options.Comments}; ";
            }

            if (!string.IsNullOrEmpty(options.Description))
            {
                attributesDefinition += $"Description={options.Description}; ";
            }

            project.Package.AttributesDefinition = attributesDefinition;

            return project;
        }

        private IEnumerable<WixEntity> FillEntities(
            Dir parent,
            IEnumerable<string> paths,
            bool withSubDirectories = true)
        {
            var added = new List<string>();
            var files = new List<File>();
            var dirs = new List<Dir>();

            foreach (var path in paths)
            {
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