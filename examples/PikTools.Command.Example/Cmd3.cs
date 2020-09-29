namespace PikTools.CommandExample
{
    using System.Collections.Generic;
    using Autodesk.Revit.Attributes;
    using Command.Api;
    using Models;
    using Shared;
    using Shared.Abstractions;
    using Shared.Ui.Abstractions;

    /// <summary>
    /// Тестовая команда проверки работы с сервисом пользовательских настроек
    /// </summary>
    [Regeneration(RegenerationOption.Manual)]
    [Transaction(TransactionMode.Manual)]
    public class Cmd3 : PikToolsCommand
    {
        /// <summary>
        /// Запуск тестовой команды
        /// </summary>
        /// <param name="userSettings">Сервис работы с пользовательскими настройками</param>
        /// <param name="notificationService">Сервис уведомлений</param>
        public PluginResult ExecuteCommand(IUserSettings userSettings, INotificationService notificationService)
        {
            var mySettings = new MySettings
            {
                StringValue = "Hello world",
                IntValue = 12,
                DoubleValue = 0.144
            };

            userSettings.Set(mySettings);
            notificationService.ShowMessage(nameof(Cmd3), "Настройки сохранены");

            var loaded = userSettings.Get<MySettings>();
            notificationService.ShowMessage(nameof(Cmd3), $"Настройки загружены со значениями: {nameof(loaded.StringValue)}: {loaded.StringValue}; {nameof(loaded.IntValue)} : {loaded.IntValue}; {nameof(loaded.DoubleValue)}: {loaded.DoubleValue}");

            var list = new List<string>
            {
                "Value 1", "Value 2", "Value 3"
            };

            userSettings.Set(list, nodeName: "MyList");
            notificationService.ShowMessage(nameof(Cmd3), "В настройки сохранена коллекция строк с именем MyList");

            var loadedList = userSettings.Get<List<string>>("MyList");
            notificationService.ShowMessage(nameof(Cmd3), $"Из настроек загружена коллекция со значениями: {string.Join(", ", loadedList)}");

            return PluginResult.Succeeded;
        }
    }
}
