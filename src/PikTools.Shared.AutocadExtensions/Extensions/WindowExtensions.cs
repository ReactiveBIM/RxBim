namespace PikTools.Shared.AutocadExtensions.Extensions
{
    using System;
    using System.Windows;
    using AcApp = Autodesk.AutoCAD.ApplicationServices.Application;
    using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

    /// <summary>
    /// Расширения для окон WPF
    /// </summary>
    public static class WindowExtensions
    {
        /// <summary>
        /// Отображает окно как модальное и отцентрированное относительно основного окна AutoCAD.
        /// Возвращает результат диалога после его закрытия.
        /// </summary>
        /// <param name="window">Окно диалога</param>
        public static bool? ShowAsModalCentered(this Window window)
        {
            FixStartupLocation(window);
            AddActivatedEvent(window);
            return Application.ShowModalWindow(null, window, false);
        }

        /// <summary>
        /// Отображает окно в виде немодального диалога, отцентрированного относительно основного окна AutoCAD
        /// </summary>
        /// <param name="window">Окно диалога</param>
        public static void ShowAsModelessCentered(this Window window)
        {
            FixStartupLocation(window);
            AddActivatedEvent(window);
            Application.ShowModelessWindow(null, window, false);
        }

        private static void FixStartupLocation(Window window)
        {
            if (window.WindowStartupLocation != WindowStartupLocation.CenterOwner)
            {
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
        }

        private static void AddActivatedEvent(Window window)
        {
            window.Activated -= WindowActivated;
            window.Activated += WindowActivated;
        }

        private static void WindowActivated(object sender, EventArgs e)
        {
            var window = (Window)sender;
            window.Activated -= WindowActivated;
            var loc = Application.MainWindow.DeviceIndependentLocation;
            var size = Application.MainWindow.DeviceIndependentSize;
            window.Top = loc.Y + size.Height / 2 - window.ActualHeight / 2;
            window.Left = loc.X + size.Width / 2 - window.ActualWidth / 2;
        }
    }
}