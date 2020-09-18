namespace PikTools.Shared.RevitExtensions.Collectors
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using PikTools.Shared.RevitExtensions.Abstractions;
    using PikTools.Shared.RevitExtensions.Extensions;
    using PikTools.Shared.RevitExtensions.Helpers;

    /// <summary>
    /// Репозиторий листов
    /// </summary>
    public class SheetsCollector : ISheetsCollector
    {
        private readonly UIDocument _uiDoc;
        private readonly Document _rootDoc;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="uiDoc">Основной документ проекта</param>
        public SheetsCollector(
            UIDocument uiDoc)
        {
            _uiDoc = uiDoc;
            _rootDoc = uiDoc.Document;
        }

        /// <inheritdoc />
        public Dictionary<string, List<string>> GetSheets()
        {
            // Получаем листы для основного документа
            var sheets = AddViewSheetByDoc(_rootDoc);

            // Выбираем связанные документы относительно основного документа
            var linkTypeNames = new FilteredElementCollector(_rootDoc)
                .WhereElementIsElementType()
                .OfCategory(BuiltInCategory.OST_RvtLinks)
                .OfClass(typeof(RevitLinkType))
                .Cast<RevitLinkType>()
                .Select(lt => Path.GetFileNameWithoutExtension(lt.Name))
                .ToList();

            // Получение листов из связанных документов
            foreach (Document doc in _rootDoc.Application.Documents)
            {
                if (linkTypeNames.Contains(doc.Title))
                {
                    var linkedSheets = AddViewSheetByDoc(doc);
                    foreach (var linkedDocSheets in linkedSheets)
                        sheets.Add(linkedDocSheets.Key, linkedDocSheets.Value);
                }
            }

            return sheets;
        }

        /// <inheritdoc />
        public Dictionary<string, Dictionary<string, List<string>>> GetSheets(string groupSheetParam)
        {
            var sheets = new Dictionary<string, Dictionary<string, List<string>>>
            {
                { _rootDoc.Title, GetGroupedViewSheets(_rootDoc, groupSheetParam) }
            };

            // Выбираем связанные документы относительно основного документа
            var linkTypeNames = new FilteredElementCollector(_rootDoc)
                .WhereElementIsElementType()
                .OfCategory(BuiltInCategory.OST_RvtLinks)
                .OfClass(typeof(RevitLinkType))
                .Cast<RevitLinkType>()
                .Select(lt => Path.GetFileNameWithoutExtension(lt.Name))
                .ToList();

            // Получение листов из связанных документов
            foreach (Document doc in _rootDoc.Application.Documents)
            {
                if (linkTypeNames.Contains(doc.Title))
                    sheets.Add(doc.Title, GetGroupedViewSheets(doc, groupSheetParam));
            }

            return sheets;
        }

        /// <inheritdoc />
        public IEnumerable<string> GetSelectedSheets()
        {
            var selectedElementIds = _uiDoc.Selection.GetElementIds();
            return GetViewSheets(_rootDoc, selectedElementIds)
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
