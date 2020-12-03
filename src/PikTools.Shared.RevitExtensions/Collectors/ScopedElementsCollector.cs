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
    using Models;

    /// <summary>
    /// Коллектор части элементов
    /// </summary>
    public class ScopedElementsCollector : IScopedElementsCollector
    {
        private readonly UIApplication _uiApplication;
        private readonly IElementsDisplay _elementsDisplay;

        private readonly Dictionary<string, List<ElementId>> _selectedElementsIds
            = new Dictionary<string, List<ElementId>>();

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="uiApplication">Current <see cref="UIApplication"/></param>
        /// <param name="elementsDisplay">Сервис показа элементов в модели</param>
        public ScopedElementsCollector(UIApplication uiApplication, IElementsDisplay elementsDisplay)
        {
            _uiApplication = uiApplication;
            _elementsDisplay = elementsDisplay;
        }

        /// <inheritdoc/>
        public ScopeType Scope { get; private set; } = ScopeType.AllModel;

        /// <inheritdoc/>
        public FilteredElementCollector GetFilteredElementCollector(
            Document doc = null, bool ignoreScope = false, bool includeSubFamilies = true)
        {
            if (doc == null)
                doc = _uiApplication.ActiveUIDocument.Document;

            // Снимаем выделение, чтобы избежать блокировки контекста Revit
            SaveAndResetSelectedElements();

            if (ignoreScope)
                return new FilteredElementCollector(doc);

            switch (Scope)
            {
                case ScopeType.SelectedElements:
                    if (!_selectedElementsIds.ContainsKey(doc.Title))
                        return new FilteredElementCollector(doc, new List<ElementId> { ElementId.InvalidElementId });

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
                        : new FilteredElementCollector(doc, new List<ElementId> { ElementId.InvalidElementId });

                case ScopeType.ActiveView:
                    return new FilteredElementCollector(doc, _uiApplication.ActiveUIDocument.ActiveGraphicalView.Id);

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
            var uiDoc = _uiApplication.ActiveUIDocument;
            var selectedIds = uiDoc.Selection.GetElementIds().ToList();
            if (!selectedIds.Any())
                return;

            if (_selectedElementsIds.ContainsKey(uiDoc.Document.Title))
                _selectedElementsIds[uiDoc.Document.Title] = selectedIds;
            else
                _selectedElementsIds.Add(uiDoc.Document.Title, selectedIds);

            _elementsDisplay.ResetSelection();
        }

        /// <inheritdoc/>
        public void SetBackSelectedElements()
        {
            var uiDoc = _uiApplication.ActiveUIDocument;
            if (_selectedElementsIds.ContainsKey(uiDoc.Document.Title))
            {
                _elementsDisplay.SetSelectedElements(
                    _selectedElementsIds[uiDoc.Document.Title].Select(e => e.IntegerValue).ToList());
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
                var uiDoc = _uiApplication.ActiveUIDocument;
                var pickRef = uiDoc.Selection.PickObject(
                    ObjectType.Element, new ElementSelectionFilter(filterElement), statusPrompt);

                // Обновляем сохраненные элементы для выбора
                if (_selectedElementsIds.ContainsKey(uiDoc.Document.Title))
                    _selectedElementsIds[uiDoc.Document.Title] = new List<ElementId> { pickRef.ElementId };
                else
                    _selectedElementsIds.Add(uiDoc.Document.Title, new List<ElementId> { pickRef.ElementId });

                return uiDoc.Document.GetElement(pickRef.ElementId);
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
                var uiDoc = _uiApplication.ActiveUIDocument;
                var pickElements = uiDoc.Selection.PickObjects(
                    ObjectType.Element, new ElementSelectionFilter(filterElement), statusPrompt)
                    .Select(r => uiDoc.Document.GetElement(r))
                    .ToList();

                // Обновляем сохраненные элементы для выбора
                if (_selectedElementsIds.ContainsKey(uiDoc.Document.Title))
                    _selectedElementsIds[uiDoc.Document.Title] = pickElements.Select(e => e.Id).ToList();
                else
                    _selectedElementsIds.Add(uiDoc.Document.Title, pickElements.Select(e => e.Id).ToList());

                return pickElements;
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return new List<Element>();
            }
        }

        /// <inheritdoc />
        public LinkedElement PickLinkedElement(Func<Element, bool> filterElement = null, string statusPrompt = "")
        {
            try
            {
                var uiDoc = _uiApplication.ActiveUIDocument;
                var doc = uiDoc.Document;
                var pickRef = uiDoc.Selection.PickObject(
                    ObjectType.LinkedElement,
                    new LinkedElementSelectionFilter(uiDoc.Document, filterElement),
                    statusPrompt);

                //// Сохранять этот элемент в _selectedElementsIds нельзя, так как RevitAPI не позволяет его добавить
                //// в UiDocument.Selection
                _elementsDisplay.ResetSelection();

                if (doc.GetElement(pickRef) is RevitLinkInstance linkInstance)
                {
                    return new LinkedElement(pickRef.LinkedElementId, linkInstance);
                }

                return null;
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return null;
            }
        }

        /// <inheritdoc />
        public List<LinkedElement> PickLinkedElements(Func<Element, bool> filterElement = null, string statusPrompt = "")
        {
            try
            {
                var uiDoc = _uiApplication.ActiveUIDocument;
                var doc = uiDoc.Document;
                var pickElements = uiDoc.Selection.PickObjects(
                        ObjectType.LinkedElement,
                        new LinkedElementSelectionFilter(uiDoc.Document, filterElement),
                        statusPrompt)
                    .Select(r => new LinkedElement(r.LinkedElementId, (RevitLinkInstance)doc.GetElement(r)))
                    .ToList();

                //// Сохранять эти элементы в _selectedElementsIds нельзя, так как RevitAPI не позволяет их добавить
                //// в UiDocument.Selection
                _elementsDisplay.ResetSelection();

                return pickElements;
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return new List<LinkedElement>();
            }
        }

        private IEnumerable<ElementId> GetSubFamilies(ElementId familyId)
        {
            var uiDoc = _uiApplication.ActiveUIDocument;
            if (!(uiDoc.Document.GetElement(familyId) is FamilyInstance familyInstance))
                yield break;

            var subFamilyIds = familyInstance.GetSubComponentIds();
            if (subFamilyIds == null)
                yield break;

            foreach (var subFamilyId in subFamilyIds)
            {
                if (!(uiDoc.Document.GetElement(subFamilyId) is FamilyInstance))
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
