namespace PikTools.CommandExample
{
    using System.Linq;
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Architecture;
    using Command.Api;
    using Shared;
    using Shared.RevitExtensions.Abstractions;
    using Shared.Ui.Abstractions;

    /// <summary>
    /// Тестовая команда проверки работы методов работы со связанными файлами в <see cref="IScopedElementsCollector"/>
    /// </summary>
    [Regeneration(RegenerationOption.Manual)]
    [Transaction(TransactionMode.Manual)]
    public class Cmd6 : PikToolsCommand
    {
        /// <summary>
        /// Запуск тестовой команды
        /// </summary>
        /// <param name="doc">Revit document</param>
        /// <param name="notificationService">Сервис уведомлений</param>
        /// <param name="elementsCollector">Коллектор элементов</param>
        public PluginResult ExecuteCommand(
            Document doc,
            INotificationService notificationService,
            IScopedElementsCollector elementsCollector)
        {
            var title = nameof(Cmd6);

            var pickedElement = elementsCollector.PickLinkedElement(element => element is Room, "Pick room");

            notificationService.ShowMessage(
                title,
                $"Выбран тип: {pickedElement.GetType().Name}");

            var peckedElements =
                elementsCollector.PickLinkedElements(element => element is Room, "Pick rooms");

            notificationService.ShowMessage(
                title,
                $"Выбрано элементов: {peckedElements.Count} типов: {string.Join(", ", peckedElements.Select(e => e.GetType().Name).Distinct())}");

            return PluginResult.Succeeded;
        }
    }
}
