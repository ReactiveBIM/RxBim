namespace RxBim.Shared.AutocadExtensions.Extensions
{
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.Runtime;

    /// <summary>
    /// Расширения для транзакций
    /// </summary>
    public static class TransactionExtensions
    {
        /// <summary>
        /// Возвращает объект, открытый с использованием транзакции и приведённый к заданному типу
        /// </summary>
        /// <param name="transaction">Транзакция</param>
        /// <param name="id">Идентификатор объекта</param>
        /// <param name="forWrite">Открыть для записи</param>
        /// <param name="openErased">Открыть, даже если объект удалён</param>
        /// <param name="forceOpenOnLockedLayer">Открыть, даже если объект находится на замороженном слое</param>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <exception cref="Exception">Если объект не соответствует заданному типу</exception>
        public static T GetObjectAs<T>(
            this Transaction transaction,
            ObjectId id,
            bool forWrite = false,
            bool openErased = false,
            bool forceOpenOnLockedLayer = true)
            where T : DBObject
        {
            if (id.Is<T>())
            {
                return (T)transaction.GetObject(
                    id,
                    forWrite ? OpenMode.ForWrite : OpenMode.ForRead,
                    openErased,
                    forceOpenOnLockedLayer);
            }

            throw new Exception(ErrorStatus.WrongObjectType, $"Объект не является типом {typeof(T)}");
        }
    }
}