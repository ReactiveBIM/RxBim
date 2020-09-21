namespace PikTools.Shared.RevitExtensions.Collectors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using Autodesk.Revit.UI.Selection;
    using PikTools.Shared.RevitExtensions.Abstractions;
    using PikTools.Shared.RevitExtensions.Helpers;

    /// <summary>
    /// Коллектор части элементов
    /// </summary>
    public class ScopedElementsCollector : IScopedElementsCollector
    {
        private readonly UIDocument _uiDoc;

        private readonly Dictionary<string, List<ElementId>> _selectedElementsIds
            = new Dictionary<string, List<ElementId>>();

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="uiDoc">Текущий видимый документ</param>
        public ScopedElementsCollector(UIDocument uiDoc)
        {
            _uiDoc = uiDoc;
        }

        /// <inheritdoc/>
        public ScopeType Scope { get; private set; } = ScopeType.AllModel;

        /// <inheritdoc/>
        public FilteredElementCollector GetFilteredElementCollector(Document doc, bool ignoreFilter = false)
        {
            if (ignoreFilter)
                return new FilteredElementCollector(doc);

            switch (Scope)
            {
                case ScopeType.SelectedElements:
                    if (!_selectedElementsIds.ContainsKey(doc.Title))
                        throw new ArgumentException("The selected elements must first be saved!");

                    var selectedIds = _selectedElementsIds[doc.Title];

                    // Вытаскиваем вложенные элементы
                    var nestedSelectedIds = new List<ElementId>();
                    foreach (var selectedId in selectedIds)
                    {
                        if (!(doc.GetElement(selectedId) is FamilyInstance selectedEl))
                            continue;
                        var nestedIds = selectedEl.GetSubComponentIds();
                        if (nestedIds != null
                            && nestedIds.Any())
                            nestedSelectedIds.AddRange(nestedIds);
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
        public void SaveSelectedElements()
        {
            var selectedIds = _uiDoc.Selection.GetElementIds().ToList();

            if (_selectedElementsIds.ContainsKey(_uiDoc.Document.Title))
                _selectedElementsIds[_uiDoc.Document.Title] = selectedIds;
            else
                _selectedElementsIds.Add(_uiDoc.Document.Title, selectedIds);

            _uiDoc.Selection.SetElementIds(new List<ElementId>());
        }

        /// <inheritdoc/>
        public void SelectElement(int id)
        {
            _uiDoc.Selection.SetElementIds(new List<ElementId> { new ElementId(id) });
        }

        /// <inheritdoc/>
        public void ZoomElement(int id)
        {
            var activeView = _uiDoc.ActiveView;
            if (activeView == null)
                return;

            var openUiViews = _uiDoc.GetOpenUIViews();

            var currentUiView = openUiViews
                .FirstOrDefault(x => x.ViewId == activeView.Id);
            if (currentUiView == null)
                return;

            var document = activeView.Document;
            var transform = Transform.Identity;
            transform.BasisX = activeView.RightDirection;
            transform.BasisY = activeView.UpDirection;
            transform.BasisZ = activeView.ViewDirection;
            transform = transform.Inverse;
            transform = Transform.Identity;
            var transfromedXyzs = new List<XYZ>();

            var element = document.GetElement(new ElementId(id));
            var boundingBox = element.get_BoundingBox(null);
            if (boundingBox == null)
                return;

            transfromedXyzs.Add(transform.OfPoint(boundingBox.Max));
            transfromedXyzs.Add(transform.OfPoint(boundingBox.Min));

            var min = transfromedXyzs.First();
            var max = transfromedXyzs.First();

            foreach (var xyz in transfromedXyzs)
            {
                if (xyz.X < min.X
                    && xyz.Y < min.Y)
                    min = xyz;
                else if (xyz.X > max.X
                    && xyz.Y > max.Y)
                    max = xyz;
            }

            currentUiView.ZoomAndCenterRectangle(min, max);
            currentUiView.Zoom(0.25);
        }

        /// <inheritdoc/>
        public void SelectSavedElements()
        {
            if (_selectedElementsIds.ContainsKey(_uiDoc.Document.Title))
                _uiDoc.Selection.SetElementIds(_selectedElementsIds[_uiDoc.Document.Title]);
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
            catch
            {
                // cansel pick
                return null;
            }
        }
    }
}
