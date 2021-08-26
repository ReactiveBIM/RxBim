namespace RxBim.Application.Ribbon.Models
{
    using System.Windows.Controls;
    using Abstractions;
    using Autodesk.Private.Windows;
    using Autodesk.Windows;
    using Di;
    using GalaSoft.MvvmLight.Command;
    using Shared;
    using Shared.Abstractions;

    /// <summary>
    /// Кнопка о программе
    /// </summary>
    public abstract class AboutButtonBase : ButtonBase, IAboutButton
    {
        private readonly IContainer _container;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="name">имя</param>
        /// <param name="text">текст</param>
        /// <param name="container"><see cref="IContainer"/></param>
        protected AboutButtonBase(string name, string text, IContainer container)
            : base(name, text, null)
        {
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
        public override IButton SetToolTip(string toolTip, bool addVersion = true)
        {
            ToolTip = toolTip;
            return this;
        }

        /// <summary>
        /// Добавляет содержимое в окно о программе
        /// </summary>
        /// <param name="content">Содержимое окна о программе</param>
        public IAboutButton SetContent(AboutBoxContent content)
        {
            Content = content;
            return this;
        }

        /// <summary>
        /// Создаёт и возвращает RibbonButton
        /// </summary>
        public RibbonButton BuildButton()
        {
            var button = new RibbonButton
            {
                Name = Name,
                Image = SmallImage,
                LargeImage = LargeImage,
                AllowInStatusBar = true,
                AllowInToolBar = true,
                GroupLocation = RibbonItemGroupLocation.Middle,
                IsEnabled = true,
                IsToolTipEnabled = true,
                IsVisible = true,
                ShowImage = true,
                ShowText = true,
                ShowToolTipOnDisabled = true,
                Text = Text,
                ToolTip = ToolTip,
                MinHeight = 0,
                MinWidth = 0,
                Size = RibbonItemSize.Large,
                ResizeStyle = RibbonItemResizeStyles.HideText,
                IsCheckable = true,
                Orientation = Orientation.Vertical,
                KeyTip = "TBC",
                CommandHandler = new RelayCommand(RibbonClick, true)
            };

            return button;
        }

        /// <summary>
        /// Отображает информацию в стандартном окне вывода CAD-приложения
        /// </summary>
        protected abstract void ShowContentInStandardMessageBox();

        /// <inheritdoc />
        protected override void SetHelpUrlInternal(string url)
        {
        }

        private void RibbonClick()
        {
            var viewer = TryGetService();
            if (viewer != null)
            {
                viewer.ShowAboutBox(Content);
            }
            else
            {
                ShowContentInStandardMessageBox();
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
                return null;
            }
        }
    }
}
