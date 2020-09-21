namespace PikTools.CommandExample.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Autodesk.Revit.DB;
    using CSharpFunctionalExtensions;
    using PikTools.CommandExample.Abstractions;
    using PikTools.Shared.RevitExtensions.Abstractions;
    using PikTools.Shared.Ui.Abstractions;

    /// <summary>
    /// my service
    /// </summary>
    public class MyService : IMyService
    {
        private readonly INotificationService _notificationService;
        private readonly IElementsCollector _elementsCollector;
        private readonly IProblemElementsStorage _problemElementsStorage;
        private readonly Document _doc;
        private readonly RevitTask _revitTask;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="notificationService">notification</param>
        /// <param name="elementsCollector">collector</param>
        /// <param name="problemElementsStorage">problems elements</param>
        /// <param name="doc">doc</param>
        /// <param name="revitTask">revit task</param>
        public MyService(
            INotificationService notificationService,
            IElementsCollector elementsCollector,
            IProblemElementsStorage problemElementsStorage,
            Document doc,
            RevitTask revitTask)
        {
            _notificationService = notificationService;
            _elementsCollector = elementsCollector;
            _problemElementsStorage = problemElementsStorage;
            _doc = doc;
            _revitTask = revitTask;
        }

        /// <summary>
        /// go
        /// </summary>
        public async Task<Result> Go()
        {
            try
            {
                var elements = _elementsCollector
                    .GetFilteredElementCollector(_doc)
                    .WhereElementIsNotElementType()
                    .ToElements()
                    .ToList();

                await _revitTask.Run((app) =>
                {
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
    }
}