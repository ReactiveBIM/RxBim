namespace RxBim.Sample.ToolTipExamples;

using System;
using System.IO;

/// <summary>
/// Provides the same offline image and table example to both host sample projects.
/// </summary>
internal static class EnhancedToolTipSampleContent
{
    private const string ImageResourceName = "RxBim.Sample.TooltipRendererPreview.png";

    /// <summary>
    /// Gets Markdown containing an embedded PNG and an aligned table.
    /// </summary>
    internal static string Markdown { get; } = CreateMarkdown();

    private static string CreateMarkdown()
    {
        using var image = typeof(EnhancedToolTipSampleContent).Assembly.GetManifestResourceStream(ImageResourceName)
            ?? throw new InvalidOperationException("The tooltip sample image resource is missing.");
        using var buffer = new MemoryStream();
        image.CopyTo(buffer);
        var imageAddress = "data:image/png;base64," + Convert.ToBase64String(buffer.ToArray());

        return "## Картинка и таблица\n\n" +
               "Изображение встроено в сборку: **интернет для его загрузки не нужен**.\n\n" +
               "![Проверка изображения RxBim](" + imageAddress + ")\n\n" +
               "### Выравнивание столбцов\n\n" +
               "| Элемент | Состояние | Количество |\n" +
               "| :--- | :---: | ---: |\n" +
               "| Картинка | Готово | 1 |\n" +
               "| Таблица | Готово | 3 |\n" +
               "| **Жирный текст** | `код` | 123 |\n\n" +
               "Проверьте границы таблицы, выравнивание и масштаб картинки. " +
               "Прокрутите карточку и откройте [документацию RxBim](https://github.com/ReactiveBIM/RxBim).\n\n" +
               "Повторите проверку у пункта **Картинка и таблица** в выпадающем меню. " +
               "Ссылка и справка по **F1** не должны запускать команду.";
    }
}