namespace PikTools.Shared.RevitExtensions.Helpers
{
    using System;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI.Selection;

    /// <inheritdoc />
    public class LinkedElementSelectionFilter : ISelectionFilter
    {
        private readonly Document _doc;
        private readonly Func<Element, bool> _filterElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkedElementSelectionFilter"/> class.
        /// </summary>
        /// <param name="doc">Родительский документ</param>
        /// <param name="filterElement">Фильтр для выбора элементов</param>
        public LinkedElementSelectionFilter(Document doc, Func<Element, bool> filterElement = null)
        {
            _doc = doc;
            _filterElement = filterElement;
        }

        /// <inheritdoc />
        public bool AllowElement(Element elem)
        {
            return true;
        }

        /// <inheritdoc />
        public bool AllowReference(Reference reference, XYZ position)
        {
            if (_doc.GetElement(reference) is RevitLinkInstance linkInstance &&
                reference.LinkedElementId != ElementId.InvalidElementId)
            {
                var docLink = linkInstance.GetLinkDocument();
                var element = docLink.GetElement(reference.LinkedElementId);
                return _filterElement?.Invoke(element) ?? true;
            }

            return false;
        }
    }
}
