namespace PikTools.Application.Ui.Api.Builder
{
    /// <summary>
    /// Закладка панели
    /// </summary>
    public class Tab : RibbonBuilder
    {
        private readonly string _tabName;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="ribbon">панель</param>
        public Tab(Ribbon ribbon)
            : base(ribbon)
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="ribbon">панель</param>
        /// <param name="tabName">имя закладки</param>
        public Tab(Ribbon ribbon, string tabName)
            : base(ribbon)
        {
            _tabName = tabName;
        }

        /// <summary>
        /// Создает панель на закладке
        /// </summary>
        /// <param name="panelTitle">имя панели</param>
        public Panel Panel(string panelTitle)
        {
            var ribbonPanel = string.IsNullOrEmpty(_tabName)
                ? Ribbon.Application.CreateRibbonPanel(panelTitle)
                : Ribbon.Application.CreateRibbonPanel(_tabName, panelTitle);

            return new Panel(Ribbon, this, ribbonPanel);
        }
    }
}