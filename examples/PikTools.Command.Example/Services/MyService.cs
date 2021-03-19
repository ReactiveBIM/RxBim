namespace PikTools.CommandExample.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Autodesk.Revit.DB;
    using CSharpFunctionalExtensions;
    using PikTools.CommandExample.Abstractions;
    using PikTools.Shared.FmHelpers.Abstractions;
    using PikTools.Shared.RevitExtensions.Abstractions;
    using PikTools.Shared.Ui.Abstractions;

    /// <inheritdoc/>
    public class MyService : IMyService
    {
        private readonly INotificationService _notificationService;
        private readonly IElementsCollector _elementsCollector;
        private readonly IProblemElementsStorage _problemElementsStorage;
        private readonly IFamilyManagerService _familyManagerService;
        private readonly ISheetsCollector _sheetsCollector;
        private readonly Document _doc;
        private readonly RevitTask _revitTask;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="notificationService">notification</param>
        /// <param name="elementsCollector">collector</param>
        /// <param name="problemElementsStorage">problems elements</param>
        /// <param name="familyManagerService">Сервис работы с FM</param>
        /// <param name="sheetsCollector"><see cref="ISheetsCollector"/></param>
        /// <param name="doc">doc</param>
        /// <param name="revitTask">revit task</param>
        public MyService(
            INotificationService notificationService,
            IElementsCollector elementsCollector,
            IProblemElementsStorage problemElementsStorage,
            IFamilyManagerService familyManagerService,
            ISheetsCollector sheetsCollector,
            Document doc,
            RevitTask revitTask)
        {
            _notificationService = notificationService;
            _elementsCollector = elementsCollector;
            _problemElementsStorage = problemElementsStorage;
            _familyManagerService = familyManagerService;
            _sheetsCollector = sheetsCollector;
            _doc = doc;
            _revitTask = revitTask;
        }

        /// <inheritdoc/>
        public async Task<Result> Go()
        {
            try
            {
                var sheets = _sheetsCollector.GetSheets("Орг.КомплектЧертежей");

                await _revitTask.Run((app) =>
                {
                    var elements = _elementsCollector
                        .GetFilteredElementCollector(_doc)
                        .WhereElementIsNotElementType()
                        .ToElements()
                        .ToList();

                    // Use Transaction

                    // Add problem element to storage
                    var problemElem = elements.FirstOrDefault();
                    if (problemElem != null)
                        _problemElementsStorage.AddProblemElement(problemElem.Id.IntegerValue, "problem description");
                });

                _notificationService.ShowMessage(GetType().FullName, _doc.Title + $" slnsajnsdanlk");

                return Result.Success();
            }
            catch (Exception exception)
            {
                return Result.Failure(exception.Message);
            }
        }

        /// <inheritdoc/>
        public Result LoadFamily(string familyName)
        {
            return _familyManagerService.GetTargetFamily(_doc, familyName);
        }
    }
}