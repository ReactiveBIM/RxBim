#pragma warning disable
namespace RxBim.Shared.Abstractions
{
    using System;

    /// <summary>
    /// Service for working with user settings.
    /// </summary>
    [Obsolete("Will be deleted in the next release!")]
    public interface IUserSettings
    {
        /// <summary>
        /// Adds or updates data in user settings. 
        /// Добавляет или обновляет данные в файле настроек. В качестве имени создаваемого узла в xml файле используется
        /// имя типа сохраняемого объекта, если не задан параметр nodeName
        /// </summary>
        /// <remarks>Передаваемый в параметре объект должен иметь свойства, позволяющие выполнить его сериализацию:
        /// <para/>- Стандартный безпараметрический конструктор
        /// <para/>- Публичные свойства с доступным геттером и сеттером (get; set;)</remarks>
        /// <param name="data">Сохраняемый/обновляемый объект</param>
        /// <param name="nodeName">Имя узла для сохранения. Рекомендуется использовать при сохранении коллекций, не
        /// являющихся частью какого-либо сохраняемого объекта</param>
        void Set(object data, string nodeName = null);

        /// <summary>
        /// Возвращает из файла настроек объект данного типа или создает новый в случае отсутствия данных или в случае
        /// возникновения ошибки
        /// </summary>
        /// <typeparam name="T">Параметр универсального типа</typeparam>
        /// <param name="nodeName">Имя узла. Используется в случае если объект сохранялся под уникальным именем.
        /// Используется обычно при сохранении коллекций, не являющихся частью какого-либо сохраняемого объекта</param>
        /// <returns>Экземпляр объекта типа T</returns>
        T Get<T>(string nodeName = null);
    }
}
