namespace RxBim.Nuke.AutoCAD
{
    using Builders;
    using Builds;
    using Generators;

    /// <inheritdoc />
    public class AutocadRxBimBuild : RxBimBuild<InstallerBuilder<AutocadPackageContentsGenerator>,
        AutocadPackageContentsGenerator, ProjectPropertiesGenerator>
    {
        //// Тут можно добавить таргеты для облегчения дебага
    }
}