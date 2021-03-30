namespace PikTools.Shared.Ui.Abstractions
{
    using CSharpFunctionalExtensions;
    using static System.Environment;

    /// <summary>
    /// Работа с внешними окнами и диалогами
    /// </summary>
    public interface IExternalDialogs
    {
        /// <summary>
        /// Показать диалог сохранения файла
        /// </summary>
        /// <param name="title">Заголовок</param>
        /// <param name="filter">Фильтр по расширениям</param>
        /// <param name="initialDirectory">Открываемая папка по-умолчанию</param>
        /// <param name="fileName">Предлагаемое имя файла</param>
        /// <param name="overwritePrompt">Выдавать предупреждение о перезаписи файла, если он уже существует</param>
        /// <returns>Путь к выбранному для сохранения файлу</returns>
        Result<string> ShowSaveFileDialog(
            string title,
            string filter,
            string initialDirectory = "",
            string fileName = "",
            bool overwritePrompt = false);

        /// <summary>
        /// Показать диалог открытия файлов
        /// </summary>
        /// <param name="filter">Фильтр файлов</param>
        /// <param name="addExtension">Добавлять ли расширение к файлу автоматически</param>
        /// <param name="multiSelect">Допускать выбор нескольких файлов</param>
        /// <returns>Массив путей к выбранным файлам</returns>
        Result<string[]> ShowOpenFileDialog(
            string filter,
            bool addExtension,
            bool multiSelect);

        /// <summary>
        /// Показать окно выбора папки
        /// </summary>
        /// <param name="description">Описание окна выбора</param>
        /// <param name="rootFolder">Начальная папка</param>
        /// <param name="selectedPath">Выбранная изначально папка</param>
        /// <returns></returns>
        Result<string> ShowFolderBrowserDialog(
            string description,
            SpecialFolder rootFolder = SpecialFolder.MyComputer,
            string selectedPath = "");

        /// <summary>
        /// Открывает проводник на заданной папке
        /// </summary>
        /// <param name="path">Путь к заданной папке</param>
        void OpenFileExplorer(string path);

        /// <summary>
        /// Открывает файл стандартным приложением
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        void OpenFile(string filePath);
    }
}
