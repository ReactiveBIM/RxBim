namespace PikTools.Shared.TableBuilder.Abstractions
{
    /// <summary>
    /// Сериализатор таблиц
    /// </summary>
    /// <typeparam name="T">Тип</typeparam>
    public interface ITableSerializer<T>
    {
        /// <summary>
        /// Сериализует таблицу в таблицу заданного типа
        /// </summary>
        /// <param name="table">Таблица</param>
        /// <param name="params">Параметры сериализации</param>
        T Serialize(Table table, ITableSerializerParameters @params);
    }
}