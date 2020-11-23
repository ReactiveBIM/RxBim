namespace PikTools.Shared.Ui.Services
{
    using System.Diagnostics;
    using CSharpFunctionalExtensions;
    using Microsoft.Win32;
    using PikTools.Shared.Ui.Abstractions;

    /// <inheritdoc/>
    public class ExternalDialogsService : IExternalDialogs
    {
        /// <inheritdoc/>
        public void OpenFileExplorer(string path)
        {
            Process.Start("explorer.exe", path);
        }

        /// <inheritdoc/>
        public Result<string> ShowSaveFileDialog(
            string title,
            string filter,
            string initialDirectory = "",
            string fileName = "")
        {
            var fileDialog = new SaveFileDialog
                {
                    Title = title,
                    Filter = filter,
                    InitialDirectory = initialDirectory,
                    FileName = fileName
                };

            return fileDialog.ShowDialog() == true
                ? fileDialog.FileName : Result.Failure<string>("Файл не выбран");
        }

        /// <inheritdoc/>
        public Result<string[]> ShowOpenFileDialog(
            string filter,
            bool addExtension = true,
            bool multiSelect = false)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = filter,
                AddExtension = addExtension,
                Multiselect = multiSelect
            };

            return openFileDialog.ShowDialog() == true
                ? openFileDialog.FileNames : Result.Failure<string[]>("Файлы не выбраны");
        }
    }
}
