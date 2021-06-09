namespace PikTools.Nuke.AutoCAD
{
    using Builds;
    using Generators;
    using Models;

    /// <inheritdoc />
    public class AutocadPikToolsBuild
        : PikToolsBuild<WixBuilder<AutocadPackageContentsGenerator>, AutocadPackageContentsGenerator,
            ProjectPropertiesGenerator>
    {
        //// Тут можно добавить таргеты для облегчения дебага
    }
}