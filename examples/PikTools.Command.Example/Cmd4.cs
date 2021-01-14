namespace PikTools.CommandExample
{
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.DB;
    using Command.Api;
    using PikTools.CommandExample.Abstractions;
    using Shared;
    using Shared.RevitExtensions.Abstractions;
    using Shared.Ui.Abstractions;

    /// <summary>
    /// Тестовая команда проверки работы <see cref="IScopedElementsCollector"/>
    /// </summary>
    [Regeneration(RegenerationOption.Manual)]
    [Transaction(TransactionMode.Manual)]
    public class Cmd4 : PikToolsCommand
    {
        /// <summary>
        /// Запуск тестовой команды
        /// </summary>
        /// <param name="doc">Revit document</param>
        /// <param name="scopedElementsCollector">Сервис коллектора части элементов</param>
        /// <param name="notificationService">Сервис уведомлений</param>
        /// <param name="myService">Сервис</param>
        public PluginResult ExecuteCommand(
            Document doc,
            IScopedElementsCollector scopedElementsCollector,
            INotificationService notificationService,
            IMyService myService)
        {
            scopedElementsCollector.SetScope(ScopeType.ActiveView);
            notificationService.ShowMessage(nameof(Cmd4), "Установили Scope в ActiveView");

            scopedElementsCollector.GetFilteredElementCollector(doc);
            notificationService.ShowMessage(nameof(Cmd4), "Использовали метод GetFilteredElementCollector. Выделение должно сброситься");

            scopedElementsCollector.SetBackSelectedElements();
            notificationService.ShowMessage(nameof(Cmd4), "Вызвали метод SetBackSelectedElements. Выделение должно вернуться");

            return PluginResult.Succeeded;
        }
    }
}
