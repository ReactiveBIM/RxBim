namespace RxBim.Shared.RevitExtensions.Models
{
    using Autodesk.Revit.DB;

    /// <summary>
    /// Данные элемента, полученного из связанного файла
    /// </summary>
    public class LinkedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkedElement"/> class.
        /// </summary>
        /// <param name="elementId">Id элемента</param>
        /// <param name="linkInstance">Экземпляр связи</param>
        public LinkedElement(ElementId elementId, RevitLinkInstance linkInstance)
        {
            Element = linkInstance.GetLinkDocument().GetElement(elementId);
            LinkInstance = linkInstance;
        }

        /// <summary>
        /// Элемент
        /// </summary>
        public Element Element { get; }
        
        /// <summary>
        /// Экземпляр связи
        /// </summary>
        public RevitLinkInstance LinkInstance { get; }
    }
}
