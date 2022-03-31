namespace RxBim.Application.Ribbon.Autocad.Abstractions
{
    using Autodesk.Windows;

    public interface IRibbonElementsService
    {
        void AddCreatedTab(RibbonTab tab);

        void AddCreatedPanel(RibbonPanel panel);

        void AddCreatedRibbonItem(RibbonItem item, RibbonItemCollection owner);
    }
}