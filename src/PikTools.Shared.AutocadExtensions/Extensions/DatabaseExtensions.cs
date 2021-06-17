namespace PikTools.Shared.AutocadExtensions.Extensions
{
    using Autodesk.AutoCAD.DatabaseServices;

    /// <summary>
    /// Расширения для базы данных
    /// </summary>
    public static class DatabaseExtensions
    {
        /// <summary>
        /// Возвращает идентификатор текстового стиля для ПИК.
        /// Если стиля в чертеже нет, возвращается идентификатор текущего стиля.
        /// </summary>
        /// <param name="db">База данных чертежа</param>
        public static ObjectId GetPikOrCurrentTextStyleId(this Database db)
        {
            const string pikTextStyleName = "PIK";
            return db.GetTextStyleIdByNameOrCurrent(pikTextStyleName);
        }

        /// <summary>
        /// Возвращает идентификатор текстового стиля по названию.
        /// Если стиля с таким названием в чертеже нет, возвращается идентификатор текущего стиля.
        /// </summary>
        /// <param name="db">База данных</param>
        /// <param name="textStyleName">Название текстового стиля</param>
        private static ObjectId GetTextStyleIdByNameOrCurrent(this Database db, string textStyleName)
        {
            using var txtStylesTable = db.TextStyleTableId.OpenAs<TextStyleTable>();
            return txtStylesTable.Has(textStyleName) ? txtStylesTable[textStyleName] : db.Textstyle;
        }
    }
}