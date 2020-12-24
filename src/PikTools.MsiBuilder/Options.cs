namespace PikTools.MsiBuilder
{
    using System.Linq;
    using System.Reflection;
    using CommandLine;

    public class Options
    {
        [Option('p', "project", Required = true, HelpText = "Set project name.")]
        public string ProjectName { get; set; }

        [Option('i', "installDir", Required = true, HelpText = "Set install directory.")]
        public string InstallDir { get; set; }

        [Option('b', "bundleDir", Required = true, HelpText = "Set bundle directory.")]
        public string BundleDir { get; set; }

        [Option('s', "sourceDir", Required = true, HelpText = "Set source directory.")]
        public string SourceDir { get; set; }

        [Option('m', "manifestDir", Required = true, HelpText = "Set manifest directory.")]
        public string ManifestDir { get; set; }

        [Option('d', "description", Required = false, HelpText = "Set description.")]
        public string Description { get; set; }

        [Option('c', "comment", Required = false, HelpText = "Set comment.")]
        public string Comments { get; set; }

        [Option('g', "packageGuid", Required = true, HelpText = "Set package guid.")]
        public string PackageGuid { get; set; }

        [Option('u', "updateCode", Required = true, HelpText = "Set update code.")]
        public string UpgradeCode { get; set; }

        [Option('v', "version", Required = true, HelpText = "Set version.")]
        public string Version { get; set; }

        [Option('o', "outDir", Required = true, HelpText = "Set output directory.")]
        public string OutDir { get; set; }

        [Option('f', "fileName", Required = true, HelpText = "Set msi file name.")]
        public string OutFileName { get; set; }

        [Option('a', "addAllAppToManifest", Required = false, HelpText = "Set need add all Application from output to manifest.")]
        public bool AddAllAppToManifest { get; set; }

        [Option('t', "projectAddingToManifest", Required = false, HelpText = "Set projects adding to manifest.")]
        public string ProjectsAddingToManifest { get; set; }

        public override string ToString()
        {
            return string.Join(" ",
                GetType()
                    .GetProperties()
                    .Select(p => (
                        val: p.GetValue(this)?.ToString(),
                        shortName: ((OptionAttribute)p.GetCustomAttribute(typeof(OptionAttribute))).ShortName)
                    )
                    .Where(tuple => !string.IsNullOrEmpty(tuple.val))
                    .Select(tuple => $"-{tuple.shortName} {tuple.val}"));
        }
    }
}