namespace PikTools.Shared.RevitExtensions.Collectors
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Abstractions;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using Extensions;
    using Helpers;

    /// <summary>
    /// Репозиторий листов
    /// </summary>
    public class SheetsCollector : ISheetsCollector
    {
        private readonly UIApplication _uiApplication;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="uiApplication">Current <see cref="UIApplication"/></param>
        public SheetsCollector(UIApplication uiApplication)
        {
            _uiApplication = uiApplication;
        }

        /// <inheritdoc />
        public Dictionary<string, List<string>> GetSheets()
        {
            var doc = _uiApplication.ActiveUIDocument.Document;

            // Получаем листы для основного документа
            var sheets = AddViewSheetByDoc(doc);

            // Выбираем связанные документы относительно основного документа
            var linkTypeNames = new FilteredElementCollector(doc)
                .WhereElementIsElementType()
                .OfCategory(BuiltInCategory.OST_RvtLinks)
                .OfClass(typeof(RevitLinkType))
                .Cast<RevitLinkType>()
                .Select(lt => Path.GetFileNameWithoutExtension(lt.Name))
                .ToList();

            // Получение листов из связанных документов
            foreach (Document d in doc.Application.Documents)
            {
                if (linkTypeNames.Contains(d.Title))
                {
                    var linkedSheets = AddViewSheetByDoc(d);
                    foreach (var linkedDocSheets in linkedSheets)
                        sheets.Add(linkedDocSheets.Key, linkedDocSheets.Value);
                }
            }

            return sheets;
        }

        /// <inheritdoc />
        public Dictionary<string, Dictionary<string, List<string>>> GetSheets(string groupSheetParam)
        {
            var doc = _uiApplication.ActiveUIDocument.Document;
            var sheets = new Dictionary<string, Dictionary<string, List<string>>>
            {
                { doc.Title, GetGroupedViewSheets(doc, groupSheetParam) }
            };

            // Выбираем связанные документы относительно основного документа
            var linkTypeNames = new FilteredElementCollector(doc)
                .WhereElementIsElementType()
                .OfCategory(BuiltInCategory.OST_RvtLinks)
                .OfClass(typeof(RevitLinkType))
                .Cast<RevitLinkType>()
                .Select(lt => Path.GetFileNameWithoutExtension(lt.Name))
                .ToList();

            // Получение листов из связанных документов
            foreach (Document d in doc.Application.Documents)
            {
                if (linkTypeNames.Contains(d.Title))
                    sheets.Add(d.Title, GetGroupedViewSheets(d, groupSheetParam));
            }

            return sheets;
        }

        /// <inheritdoc />
        public IEnumerable<string> GetSelectedSheets()
        {
            var uiDoc = _uiApplication.ActiveUIDocument;
            var doc = uiDoc.Document;
            var selectedElementIds = uiDoc.Selection.GetElementIds();
            return GetViewSheets(doc, selectedElementIds)
                .Select(sheet => sheet.Title);
        }

        private Dictionary<string, List<string>> AddViewSheetByDoc(Document doc)
        {
            var sheets = new Dictionary<string, List<string>>();

            var views = GetViewSheets(doc);
            if (views.Any())
            {
                sheets.Add(
                    doc.Title,
                    views
                        .OrderBy(view => view.SheetNumber, new SemiNumericComparer())
                        .Select(view => view.Title)
                        .ToList());
            }

            return sheets;
        }

        private Dictionary<string, List<string>> GetGroupedViewSheets(
            Document doc, string groupSheetParam)
        {
            var sheets = new Dictionary<string, List<string>>();

            var views = GetViewSheets(doc);
            if (views.Any())
            {
                sheets = views.GroupBy(v => v.LookupParameter(groupSheetParam).GetParameterValue().ToString())
                            .ToDictionary(g => g.Key, g => g.OrderBy(view => view.SheetNumber, new SemiNumericComparer())
                                .Select(view => view.Title)
                                .ToList());
            }

            return sheets;
        }

        /// <summary>
        /// Получить список листов документа
        /// </summary>
        /// <param name="doc">Документ Revit</param>
        /// <param name="selectedElems">Выбранные элементы на листе</param>
        /// <returns>Список листов документа</returns>
        private List<ViewSheet> GetViewSheets(Document doc, ICollection<ElementId> selectedElems = null)
        {
            FilteredElementCollector collector;
            if (selectedElems != null)
            {
                if (selectedElems.Any())
                    collector = new FilteredElementCollector(doc, selectedElems);
                else
                    return new List<ViewSheet>();
            }
            else
            {
                collector = new FilteredElementCollector(doc);
            }

            return collector
                .OfClass(typeof(ViewSheet))
                .Cast<ViewSheet>()
                .Where(sheet => !sheet.IsPlaceholder
                                && sheet.CanBePrinted)
                .ToList();
        }
    }
}
