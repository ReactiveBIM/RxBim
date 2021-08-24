namespace RxBim.Shared.CivilExtensions.Extensions
{
    using System;
    using Autodesk.Civil.DatabaseServices;

    /// <summary>
    /// Расширения для примитивов Civil 3D
    /// </summary>
    public static class CivilEntityExtensions
    {
        /// <summary>
        /// Возвращает истину, если примитив является ссылкой
        /// </summary>
        /// <param name="entity">Примитив</param>
        public static bool IsShortcutReference(this Entity entity)
        {
            return entity.IsReferenceObject || entity.IsReferenceSubObject;
        }

        /// <summary>
        /// Редактирование объекта Civil
        /// </summary>
        /// <param name="ent">Объект Civil</param>
        /// <param name="edit">Действие с объектом</param>
        /// <typeparam name="T">Тип объекта Civil</typeparam>
        public static void Edit<T>(this T ent, Action<T> edit)
            where T : Entity
        {
            if (ent == null || ent.IsShortcutReference())
            {
                return;
            }

            edit(ent);
        }

        /// <summary>
        /// Редактирование объекта Civil
        /// </summary>
        /// <param name="ent">Объект Civil</param>
        /// <param name="edit">Действие с объектом</param>
        public static void Edit(this Entity ent, Action edit)
        {
            if (ent == null || ent.IsShortcutReference())
            {
                return;
            }

            edit();
        }
    }
}