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
    /// Base implementation for <see cref="IAboutButton"/>
    /// </summary>
    public abstract class AboutButtonBase : ButtonBase, IAboutButton
    {
        private readonly IContainer _container;
        private readonly RibbonButton _aboutRibbonButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="AboutButtonBase"/> class.
        /// </summary>
        /// <param name="aboutRibbonButton">About ribbon button</param>
        /// <param name="container">DI container</param>
        protected AboutButtonBase(RibbonButton aboutRibbonButton, IContainer container)
            : base(null)
        {
            _aboutRibbonButton = aboutRibbonButton;
            _container = container;

            _aboutRibbonButton.AllowInStatusBar = true;
            _aboutRibbonButton.AllowInToolBar = true;
            _aboutRibbonButton.GroupLocation = RibbonItemGroupLocation.Middle;
            _aboutRibbonButton.IsEnabled = true;
            _aboutRibbonButton.IsToolTipEnabled = false;
            _aboutRibbonButton.IsVisible = true;
            _aboutRibbonButton.ShowImage = true;
            _aboutRibbonButton.ShowText = true;
            _aboutRibbonButton.ShowToolTipOnDisabled = false;
            _aboutRibbonButton.MinHeight = 0;
            _aboutRibbonButton.MinWidth = 0;
            _aboutRibbonButton.Size = RibbonItemSize.Large;
            _aboutRibbonButton.ResizeStyle = RibbonItemResizeStyles.HideText;
            _aboutRibbonButton.IsCheckable = true;
            _aboutRibbonButton.Orientation = Orientation.Vertical;
            _aboutRibbonButton.CommandHandler = new RelayCommand(RibbonClick, true);
        }

        /// <summary>
        /// About window content
        /// </summary>
        protected AboutBoxContent Content { get; private set; }

        /// <inheritdoc />
        public override IButton SetToolTip(string toolTip, bool addVersion = true, string versionHeader = "")
        {
            SetTooltipInternal(toolTip);
            return this;
        }

        /// <inheritdoc />
        public IAboutButton SetContent(AboutBoxContent content)
        {
            Content = content;
            return this;
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

        /// <inheritdoc />
        protected override void SetTooltipInternal(string tooltip)
        {
            _aboutRibbonButton.IsToolTipEnabled = true;
            _aboutRibbonButton.ShowToolTipOnDisabled = true;
            _aboutRibbonButton.ToolTip = tooltip;
        }

        private void RibbonClick()
        {
            if (Content is null)
                return;

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