namespace PikTools.Shared.AutocadExtensions.Helpers
{
    using System;
    using Autodesk.AutoCAD.DatabaseServices;

    /// <summary>
    /// Класс для временного переключения рабочей БД
    /// </summary>
    public class WorkingDatabaseSwitcher : IDisposable
    {
        /// <summary>
        /// Метка того, нужно переключать БД или нет
        /// </summary>
        private readonly bool _needSwitch;

        /// <summary>
        /// Переменная для хранения исходной БД
        /// </summary>
        private readonly Database _oldWorkDb;

        /// <summary>
        /// Создание вспомогательного объекта для временного переключения БД
        /// </summary>
        /// <param name="tmpWorkDb">Ссылка на БД на которую временно переключаемся</param>
        public WorkingDatabaseSwitcher(Database tmpWorkDb)
        {
            _oldWorkDb = HostApplicationServices.WorkingDatabase;
            _needSwitch = !tmpWorkDb.Equals(_oldWorkDb);
            if (_needSwitch)
            {
                HostApplicationServices.WorkingDatabase = tmpWorkDb;
            }
        }

        /// <summary>
        /// Реализация IDisposable
        /// </summary>
        public void Dispose()
        {
            if (_needSwitch)
            {
                HostApplicationServices.WorkingDatabase = _oldWorkDb;
            }
        }
    }
}