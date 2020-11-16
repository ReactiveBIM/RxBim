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

            if (fileDialog.ShowDialog() == true)
                return fileDialog.FileName;

            return Result.Failure<string>("Файл не выбран");
        }
    }
}
