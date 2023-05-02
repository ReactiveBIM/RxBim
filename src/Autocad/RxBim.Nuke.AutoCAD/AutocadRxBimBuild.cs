namespace RxBim.Nuke.AutoCAD
{
    using Builders;
    using Builds;
    using Generators;

    /// <inheritdoc />
    public class AutocadRxBimBuild : RxBimBuild<
        InstallerBuilder<AutocadPackageContentsGenerator, OptionsBuilder>,
        AutocadPackageContentsGenerator,
        ProjectPropertiesGenerator,
        OptionsBuilder>
    {
    }
}