namespace PikTools.Shared.RevitExtensions.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Abstractions;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;

    /// <inheritdoc />
    public class ElementsDisplayService : IElementsDisplay
    {
        private readonly UIApplication _uiApplication;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementsDisplayService"/> class.
        /// </summary>
        /// <param name="uiApplication">Current <see cref="UIApplication"/></param>
        public ElementsDisplayService(UIApplication uiApplication)
        {
            _uiApplication = uiApplication;
        }

        /// <inheritdoc />
        public void SetSelectedElements(IList<int> elementIds)
        {
            _uiApplication.ActiveUIDocument.Selection.SetElementIds(elementIds.Select(e => new ElementId(e)).ToList());
        }

        /// <inheritdoc />
        public void SetSelectedElement(int elementId)
        {
            _uiApplication.ActiveUIDocument.Selection.SetElementIds(new List<ElementId> { new ElementId(elementId) });
        }

        /// <inheritdoc />
        public void ResetSelection()
        {
            _uiApplication.ActiveUIDocument.Selection.SetElementIds(new List<ElementId>());
        }

        /// <inheritdoc />
        public void ZoomElement(int elementId, double zoomFactor = 0.25)
        {
            var activeView = _uiApplication.ActiveUIDocument.ActiveView;
            if (activeView == null)
                return;

            var openUiViews = _uiApplication.ActiveUIDocument.GetOpenUIViews();

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

            var element = document.GetElement(new ElementId(elementId));
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
            currentUiView.Zoom(zoomFactor);
        }
    }
}
