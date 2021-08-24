namespace RxBim.Shared.AutocadExtensions.Extensions
{
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.Runtime;

    /// <summary>
    /// Расширения для идентификаторов объектов
    /// </summary>
    public static class ObjectIdExtensions
    {
        /// <summary>
        /// Возвращает истину, если идентификатор объекта полностью валидный: объект есть в базе и он не удалён
        /// </summary>
        /// <param name="id">Идентификатор</param>
        public static bool IsFullyValid(this ObjectId id)
        {
            return id.IsValid && !id.IsNull && !id.IsErased && !id.IsEffectivelyErased;
        }

        /// <summary>
        /// Возвращает истину, если объект соответствует заданному типу
        /// </summary>
        /// <typeparam name="T">Тип, на который проверяем</typeparam>
        /// <param name="id">Идентификатор объекта объекта</param>
        public static bool Is<T>(this ObjectId id)
            where T : DBObject
        {
            if (!id.IsValid)
            {
                return false;
            }

            RXClass
                rxClass = RXObject.GetClass(typeof(T)),
                objClass = id.ObjectClass;

            return objClass.Equals(rxClass)
                   || objClass.IsDerivedFrom(rxClass);
        }

        /// <summary>
        /// Возвращает объект, открытый без использования транзакции и приведённый к заданному типу
        /// </summary>
        /// <param name="id">Идентификатор объекта</param>
        /// <param name="forWrite">Открыть для записи</param>
        /// <param name="openErased">Открыть, даже если объект удалён</param>
        /// <param name="forceOpenOnLockedLayer">Открыть, даже если объект находится на замороженном слое</param>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <exception cref="Exception">Если объект не соответствует заданному типу</exception>
        public static T OpenAs<T>(
            this ObjectId id,
            bool forWrite = false,
            bool openErased = false,
            bool forceOpenOnLockedLayer = true)
            where T : DBObject
        {
            if (id.Is<T>())
            {
#pragma warning disable 618
                return (T)id.Open(
                    forWrite ? OpenMode.ForWrite : OpenMode.ForRead,
                    openErased,
                    forceOpenOnLockedLayer);
#pragma warning restore 618
            }

            throw new Exception(ErrorStatus.WrongObjectType, $"Объект не является типом {typeof(T)}");
        }

        /// <summary>
        /// Возвращает объект, открытый с использованием транзакции и приведённый к заданному типу.
        /// Для работы метода необходимо, чтобы была запущена транзакция!
        /// </summary>
        /// <param name="id">Идентификатор объекта</param>
        /// <param name="forWrite">Открыть для записи</param>
        /// <param name="openErased">Открыть, даже если объект удалён</param>
        /// <param name="forceOpenOnLockedLayer">Открыть, даже если объект находится на замороженном слое</param>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <exception cref="Exception">Если объект не соответствует заданному типу</exception>
        public static T GetObjectAs<T>(
            this ObjectId id,
            bool forWrite = false,
            bool openErased = false,
            bool forceOpenOnLockedLayer = true)
            where T : DBObject
        {
            if (id.Is<T>())
            {
                return (T)id.GetObject(
                    forWrite ? OpenMode.ForWrite : OpenMode.ForRead,
                    openErased,
                    forceOpenOnLockedLayer);
            }

            throw new Exception(ErrorStatus.WrongObjectType, $"Объект не является типом {typeof(T)}");
        }
    }
}