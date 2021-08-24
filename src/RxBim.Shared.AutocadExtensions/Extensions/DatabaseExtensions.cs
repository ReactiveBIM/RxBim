namespace RxBim.Shared.AutocadExtensions.Extensions
{
    using Autodesk.AutoCAD.DatabaseServices;

    /// <summary>
    /// Расширения для базы данных
    /// </summary>
    public static class DatabaseExtensions
    {
        /// <summary>
        /// Возвращает идентификатор текстового стиля по названию.
        /// Если стиля с таким названием в чертеже нет, возвращается идентификатор текущего стиля.
        /// </summary>
        /// <param name="db">База данных</param>
        /// <param name="textStyleName">Название текстового стиля</param>
        public static ObjectId GetTextStyleId(this Database db, string textStyleName = "PIK")
        {
            using var txtStylesTable = db.TextStyleTableId.OpenAs<TextStyleTable>();
            return txtStylesTable.Has(textStyleName) ? txtStylesTable[textStyleName] : db.Textstyle;
        }
    }
}