namespace RxBim.Nuke.AutoCAD
{
    using Builders;
    using Builds;
    using Generators;

    /// <inheritdoc />
    public class AutocadRxBimBuild
        : RxBimBuild<WixBuilder<AutocadPackageContentsGenerator>, AutocadPackageContentsGenerator,
            ProjectPropertiesGenerator>
    {
        //// Тут можно добавить таргеты для облегчения дебага
    }
}