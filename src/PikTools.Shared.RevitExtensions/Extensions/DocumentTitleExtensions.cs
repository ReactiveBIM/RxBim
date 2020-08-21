namespace PikTools.Shared.RevitExtensions.Extensions
{
    using System.Linq;
    using Autodesk.Revit.DB;

    /// <summary>
    /// Расширения для заголовка документа Revit
    /// </summary>
    public static class DocumentTitleExtensions
    {
        private const char DocTitleSeparator = '-';
        private const char DocTitleEndSeparator = '_';

        /// <summary>
        /// Получить стадию документа
        /// </summary>
        /// <param name="doc">Текущий документ</param>
        /// <returns>Стадия документа</returns>
        public static string GetDocumentStage(this Document doc)
        {
            var titleValues = doc.Title.Split(DocTitleSeparator);

            return titleValues.Length < 2
                ? string.Empty
                : titleValues[1];
        }

        /// <summary>
        /// Получить дисциплину документа
        /// </summary>
        /// <param name="doc">Текущий документ</param>
        /// <returns>Дисциаплина документа</returns>
        public static string GetDocumentDiscipline(this Document doc)
        {
            var titleValues = doc.Title.Split(DocTitleSeparator);

            return titleValues.Length < 5
                ? string.Empty
                : titleValues[4];
        }

        /// <summary>
        /// Получить значение раздела документа
        /// </summary>
        /// <param name="doc">Текущий документ</param>
        /// <returns>Значение раздела документа</returns>
        public static string GetDocumentPartition(this Document doc)
        {
            var titleValues = doc.Title.Split(DocTitleSeparator);

            return titleValues.Length < 6
                ? string.Empty
                : titleValues[5].Split(DocTitleEndSeparator).FirstOrDefault();
        }
    }
}
