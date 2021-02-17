namespace PikTools.CommandExample
{
    using System;
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.DB;
    using Command.Api;
    using Shared;
    using Shared.FmHelpers.Abstractions;
    using Shared.RevitExtensions.Abstractions;
    using Shared.Ui.Abstractions;

    /// <summary>
    /// Тестовая команда проверки работы методов работы со связанными файлами в <see cref="IScopedElementsCollector"/>
    /// </summary>
    [Regeneration(RegenerationOption.Manual)]
    [Transaction(TransactionMode.Manual)]
    public class Cmd7 : PikToolsCommand
    {
        /// <summary>
        /// Запуск тестовой команды
        /// </summary>
        /// <param name="doc">Revit document</param>
        /// <param name="notificationService">Сервис уведомлений</param>
        /// <param name="familyManagerService">Сервис по работе с общими параметрами</param>
        public PluginResult ExecuteCommand(
            Document doc,
            INotificationService notificationService,
            IFamilyManagerService familyManagerService)
        {
            var a = familyManagerService.GetFamiliesByFunctionalType(doc, "Окна");

            return PluginResult.Succeeded;
        }
    }
}