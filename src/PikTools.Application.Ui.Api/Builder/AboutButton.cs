namespace PikTools.Application.Ui.Api.Builder
{
    using System.Windows.Controls;
    using Autodesk.Private.Windows;
    using Autodesk.Windows;
    using PikTools.Di;
    using PikTools.Shared;
    using PikTools.Shared.Abstractions;
    using TaskDialog = Autodesk.Revit.UI.TaskDialog;

    /// <summary>
    /// Кнопка о программе
    /// </summary>
    public class AboutButton : Button
    {
        private readonly string _id;
        private readonly IContainer _container;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="name">имя</param>
        /// <param name="text">текст</param>
        /// <param name="id">Идентификатор кнопки</param>
        /// <param name="container"><see cref="IContainer"/></param>
        public AboutButton(string name, string text, string id, IContainer container)
            : base(name, text, null)
        {
            _id = id;
            _container = container;
        }

        /// <summary>
        /// Содержимое окна о программе
        /// </summary>
        protected AboutBoxContent Content { get; set; }

        /// <summary>
        /// Добавляет всплывающее описание кнопки
        /// </summary>
        /// <param name="toolTip">Текст всплывающего описания</param>
        /// <param name="addVersion">Флаг добавления версии</param>
        public override Button SetToolTip(string toolTip, bool addVersion = true)
        {
            ToolTip = toolTip;
            return this;
        }

        /// <summary>
        /// Добавляет содержимое в окно о программе
        /// </summary>
        /// <param name="content">Содержимое окна о программе</param>
        public AboutButton SetContent(AboutBoxContent content)
        {
            Content = content;
            return this;
        }

        /// <summary>
        /// Заканчивает настройку кнопки
        /// </summary>
        internal RibbonButton BuildButton()
        {
            var button = new RibbonButton();

            button.Name = Name;
            button.Image = SmallImage;
            button.LargeImage = LargeImage;
            button.Id = _id;
            button.AllowInStatusBar = true;
            button.AllowInToolBar = true;
            button.GroupLocation = RibbonItemGroupLocation.Middle;
            button.IsEnabled = true;
            button.IsToolTipEnabled = true;
            button.IsVisible = true;
            button.ShowImage = true;
            button.ShowText = true;
            button.ShowToolTipOnDisabled = true;
            button.Text = Text;
            button.ToolTip = ToolTip;
            button.MinHeight = 0;
            button.MinWidth = 0;
            button.Size = RibbonItemSize.Large;
            button.ResizeStyle = RibbonItemResizeStyles.HideText;
            button.IsCheckable = true;
            button.Orientation = Orientation.Vertical;
            button.KeyTip = "TBC";

            ComponentManager.UIElementActivated += RibbonClick;

            return button;
        }

        private void RibbonClick(object sender, UIElementActivatedEventArgs e)
        {
            if (e.Item is RibbonButton button
                && button.Id.Equals(_id))
            {
                var viewer = TryGetService();
                if (viewer != null)
                {
                    viewer.ShowAboutBox(Content);
                }
                else
                {
                    TaskDialog.Show(Name, Content?.ToString());
                }
            }
        }

        private IAboutShowService TryGetService()
        {
            try
            {
                return _container.GetService<IAboutShowService>();
            }
            catch
            {
                // ignor
            }

            return null;
        }
    }
}
