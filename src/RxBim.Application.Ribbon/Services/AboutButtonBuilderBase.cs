namespace RxBim.Application.Ribbon.Services
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
    /// Base implementation for <see cref="IAboutButtonBuilder"/>
    /// </summary>
    public abstract class AboutButtonBuilderBase : ButtonBuilderBase, IAboutButtonBuilder
    {
        private readonly IContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="AboutButtonBuilderBase"/> class.
        /// </summary>
        /// <param name="name">Button name</param>
        /// <param name="text">Button label text</param>
        /// <param name="container">DI container</param>
        protected AboutButtonBuilderBase(string name, string text, IContainer container)
            : base(name, text, null)
        {
            _container = container;
        }

        /// <summary>
        /// About window content
        /// </summary>
        protected AboutBoxContent Content { get; set; }

        /// <inheritdoc />
        public override IButtonBuilder SetToolTip(string toolTip, bool addVersion = true, string versionInfoHeader = "")
        {
            ToolTip = toolTip;
            return this;
        }

        /// <inheritdoc />
        public IAboutButtonBuilder SetContent(AboutBoxContent content)
        {
            Content = content;
            return this;
        }

        /// <summary>
        /// Creates and returns RibbonButton
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
                CommandHandler = new RelayCommand(RibbonClick, true)
            };

            return button;
        }

        /// <summary>
        /// Displays information in the standard message box of the CAD application
        /// </summary>
        protected abstract void ShowContentInStandardMessageBox();

        /// <inheritdoc />
        protected override void SetHelpUrlInternal(string url)
        {
            // no help url
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
