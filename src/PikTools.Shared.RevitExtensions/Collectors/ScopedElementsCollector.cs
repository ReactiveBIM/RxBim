namespace PikTools.Shared.RevitExtensions.Collectors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Abstractions;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using Autodesk.Revit.UI.Selection;
    using Helpers;

    /// <summary>
    /// Коллектор части элементов
    /// </summary>
    public class ScopedElementsCollector : IScopedElementsCollector
    {
        private readonly UIDocument _uiDoc;
        private readonly IElementsDisplay _elementsDisplay;

        private readonly Dictionary<string, List<ElementId>> _selectedElementsIds
            = new Dictionary<string, List<ElementId>>();

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="uiDoc">Текущий видимый документ</param>
        /// <param name="elementsDisplay">Сервис показа элементов в модели</param>
        public ScopedElementsCollector(UIDocument uiDoc, IElementsDisplay elementsDisplay)
        {
            _uiDoc = uiDoc;
            _elementsDisplay = elementsDisplay;
        }

        /// <inheritdoc/>
        public ScopeType Scope { get; private set; } = ScopeType.AllModel;

        /// <inheritdoc/>
        public FilteredElementCollector GetFilteredElementCollector(
            Document doc, bool ignoreScope = false, bool includeSubFamilies = true)
        {
            // Снимаем выделение, чтобы избежать блокировки контекста Revit
            SaveAndResetSelectedElements();

            if (ignoreScope)
                return new FilteredElementCollector(doc);

            switch (Scope)
            {
                case ScopeType.SelectedElements:
                    if (!_selectedElementsIds.ContainsKey(doc.Title))
                        return null;

                    var selectedIds = _selectedElementsIds[doc.Title];

                    // Вытаскиваем вложенные элементы
                    var nestedSelectedIds = new List<ElementId>();
                    foreach (var selectedId in selectedIds)
                    {
                        if (includeSubFamilies)
                            nestedSelectedIds.AddRange(GetSubFamilies(selectedId));
                    }

                    selectedIds.AddRange(nestedSelectedIds);
                    return selectedIds.Any()
                        ? new FilteredElementCollector(doc, selectedIds)
                        : null;

                case ScopeType.ActiveView:
                    return new FilteredElementCollector(doc, _uiDoc.ActiveGraphicalView.Id);

                default:
                    return new FilteredElementCollector(doc);
            }
        }

        /// <inheritdoc/>
        public bool HasElements(Document doc)
        {
            return GetFilteredElementCollector(doc)
                ?.WhereElementIsNotElementType()
                .Any() ?? false;
        }

        /// <inheritdoc/>
        public void SaveAndResetSelectedElements()
        {
            var selectedIds = _uiDoc.Selection.GetElementIds().ToList();
            if (!selectedIds.Any())
                return;

            if (_selectedElementsIds.ContainsKey(_uiDoc.Document.Title))
                _selectedElementsIds[_uiDoc.Document.Title] = selectedIds;
            else
                _selectedElementsIds.Add(_uiDoc.Document.Title, selectedIds);

            _elementsDisplay.ResetSelection();
        }

        /// <inheritdoc/>
        public void SetBackSelectedElements()
        {
            if (_selectedElementsIds.ContainsKey(_uiDoc.Document.Title))
            {
                _elementsDisplay.SetSelectedElements(
                    _selectedElementsIds[_uiDoc.Document.Title].Select(e => e.IntegerValue).ToList());
            }
        }

        /// <inheritdoc/>
        public void SetScope(ScopeType scope)
        {
            Scope = scope;
        }

        /// <inheritdoc/>
        public Element PickElement(Func<Element, bool> filterElement = null, string statusPrompt = "")
        {
            try
            {
                var pickRef = _uiDoc.Selection.PickObject(
                    ObjectType.Element, new ElementSelectionFilter(filterElement), statusPrompt);

                // Обновляем сохраненные элементы для выбора
                if (_selectedElementsIds.ContainsKey(_uiDoc.Document.Title))
                    _selectedElementsIds[_uiDoc.Document.Title] = new List<ElementId> { pickRef.ElementId };
                else
                    _selectedElementsIds.Add(_uiDoc.Document.Title, new List<ElementId> { pickRef.ElementId });

                return _uiDoc.Document.GetElement(pickRef.ElementId);
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return null;
            }
        }

        /// <inheritdoc />
        public List<Element> PickElements(Func<Element, bool> filterElement = null, string statusPrompt = "")
        {
            try
            {
                var pickElements = _uiDoc.Selection.PickObjects(
                    ObjectType.Element, new ElementSelectionFilter(filterElement), statusPrompt)
                    .Select(r => _uiDoc.Document.GetElement(r))
                    .ToList();

                // Обновляем сохраненные элементы для выбора
                if (_selectedElementsIds.ContainsKey(_uiDoc.Document.Title))
                    _selectedElementsIds[_uiDoc.Document.Title] = pickElements.Select(e => e.Id).ToList();
                else
                    _selectedElementsIds.Add(_uiDoc.Document.Title, pickElements.Select(e => e.Id).ToList());

                return pickElements;
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return null;
            }
        }

        private void SaveSelectedElements()
        {
            var selectedIds = _uiDoc.Selection.GetElementIds().ToList();

            if (_selectedElementsIds.ContainsKey(_uiDoc.Document.Title))
                _selectedElementsIds[_uiDoc.Document.Title] = selectedIds;
            else
                _selectedElementsIds.Add(_uiDoc.Document.Title, selectedIds);
        }

        private IEnumerable<ElementId> GetSubFamilies(ElementId familyId)
        {
            if (!(_uiDoc.Document.GetElement(familyId) is FamilyInstance familyInstance))
                yield break;

            var subFamilyIds = familyInstance.GetSubComponentIds();
            if (subFamilyIds == null)
                yield break;

            foreach (var subFamilyId in subFamilyIds)
            {
                if (!(_uiDoc.Document.GetElement(subFamilyId) is FamilyInstance))
                    continue;

                yield return subFamilyId;

                foreach (var family in GetSubFamilies(subFamilyId))
                {
                    yield return family;
                }
            }
        }
    }
}
