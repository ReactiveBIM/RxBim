namespace RxBim.Shared.TableBuilder
{
    /// <summary> Билдер </summary>
    public abstract class TableBuilderBase
    {
        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="table">Таблица</param>
        protected TableBuilderBase(Table table = null)
        {
            Table = table ?? new Table();
        }

        /// <summary> Операция And. Возвращает таблицу </summary>
        public Table And => Table;

        /// <summary> Таблица </summary>
        protected Table Table { get; }
    }
}