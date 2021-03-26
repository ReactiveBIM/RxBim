namespace PikTools.Shared.Ui.Services
{
    using System.Diagnostics;
    using System.Windows.Forms;
    using CSharpFunctionalExtensions;
    using PikTools.Shared.Ui.Abstractions;
    using static System.Environment;
    using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
    using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

    /// <inheritdoc/>
    public class ExternalDialogsService : IExternalDialogs
    {
        /// <inheritdoc/>
        public void OpenFile(string filePath)
        {
            Process.Start(filePath);
        }

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
            string fileName = "",
            bool overwritePrompt = true)
        {
            var fileDialog = new SaveFileDialog
                {
                    Title = title,
                    Filter = filter,
                    InitialDirectory = initialDirectory,
                    FileName = fileName,
                    OverwritePrompt = overwritePrompt
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

        /// <inheritdoc/>
        public Result<string> ShowFolderBrowserDialog(
            string description,
            SpecialFolder rootFolder = SpecialFolder.MyComputer,
            string selectedPath = "")
        {
            using var directoryDialog = new FolderBrowserDialog()
            {
                Description = description,
                RootFolder = rootFolder,
                SelectedPath = selectedPath
            };

            return directoryDialog.ShowDialog() == DialogResult.OK
                ? directoryDialog.SelectedPath
                : Result.Failure<string>("Папка не выбрана");
        }
    }
}
