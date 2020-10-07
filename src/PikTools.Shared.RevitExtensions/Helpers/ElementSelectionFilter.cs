namespace PikTools.Shared.RevitExtensions.Helpers
{
    using System;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI.Selection;

    /// <inheritdoc/>
    internal class ElementSelectionFilter : ISelectionFilter
    {
        private readonly Func<Element, bool> _filterElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementSelectionFilter"/> class.
        /// </summary>
        /// <param name="filterElement">Фильтр для выбора элементов</param>
        public ElementSelectionFilter(Func<Element, bool> filterElement = null)
        {
            _filterElement = filterElement;
        }

        /// <inheritdoc/>
        public bool AllowElement(Element elem)
        {
            return _filterElement?.Invoke(elem) ?? true;
        }

        /// <inheritdoc/>
        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }
}
