namespace PikTools.Shared.RevitExtensions.Collectors
{
    using System.Collections.Generic;
    using System.Linq;
    using Autodesk.Revit.DB;
    using PikTools.Shared.RevitExtensions.Abstractions;

    /// <summary>
    /// Коллектор документов
    /// </summary>
    public class DocumentsCollector : IDocumentsCollector
    {
        private readonly Document _doc;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="doc">Основной документ Revit</param>
        public DocumentsCollector(Document doc)
        {
            _doc = doc;
        }

        /// <inheritdoc/>
        public IEnumerable<string> GetDocumentsTitles()
        {
            var titles = new FilteredElementCollector(_doc)
                .OfClass(typeof(RevitLinkInstance))
                .Cast<RevitLinkInstance>()
                .Where(l => IsNotNestedLib(l))
                .Select(l => l.GetLinkDocument())
                .Where(d => d != null)
                .Select(d => d.Title)
                .ToList();
            titles.Insert(0, _doc.Title);

            return titles;
        }

        /// <inheritdoc/>
        public string GetMainDocumentTitle()
        {
            return _doc.Title;
        }

        private bool IsNotNestedLib(RevitLinkInstance linkInstance)
        {
            var linkType = _doc.GetElement(linkInstance.GetTypeId()) as RevitLinkType;
            return linkType.GetLinkedFileStatus() == LinkedFileStatus.Loaded
                   && !linkType.IsNestedLink;
        }
    }
}
