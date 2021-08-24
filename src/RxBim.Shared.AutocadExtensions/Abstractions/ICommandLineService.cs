namespace RxBim.Shared.AutocadExtensions.Abstractions
{
    /// <summary>
    /// Сервис для командной строки
    /// </summary>
    public interface ICommandLineService
    {
        /// <summary>
        /// Выводит текстовое сообщение как новую строку
        /// </summary>
        /// <param name="message">Сообщение</param>
        void WriteAsNewLine(string message);
    }
}