namespace PikTools.Shared.AutocadExtensions.Extensions
{
    using System;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.Runtime;
    using JetBrains.Annotations;
    using Exception = Autodesk.AutoCAD.Runtime.Exception;

    /// <summary>
    /// Расширения для идентификаторов объектов
    /// </summary>
    [PublicAPI]
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
        /// Открывает без транзакции объект заданного типа и возвращает его
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="id">Идентификатор объекта</param>
        /// <param name="forWrite">Открыть для записи</param>
        /// <param name="openErased">Открыть, даже если объект удалён</param>
        /// <param name="forceOpenOnLockedLayer">Открыть, даже если объект находится на замороженном слое</param>
        public static T OpenAs<T>(
            this ObjectId id,
            bool forWrite = false,
            bool openErased = false,
            bool forceOpenOnLockedLayer = true)
            where T : DBObject
        {
            if (id.Is<T>())
            {
                try
                {
#pragma warning disable 618
                    return id.Open(
                        forWrite ? OpenMode.ForWrite : OpenMode.ForRead,
                        openErased,
                        forceOpenOnLockedLayer) as T;
#pragma warning restore 618
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return null;
        }
    }
}