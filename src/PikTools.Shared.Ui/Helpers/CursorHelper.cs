namespace PikTools.Shared.Ui.Helpers
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interop;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Microsoft.Win32.SafeHandles;

    /// <summary>
    /// Преобразователь UIElement в картинку для курсора
    /// </summary>
    public class CursorHelper
    {
        /// <summary>
        /// Преобразовать UIElement в картинку для курсора
        /// </summary>
        /// <param name="element">UIElement</param>
        /// <returns>Картинка для курсора</returns>
        public static Cursor CreateCursor(UIElement element)
        {
            element.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            element.Arrange(new Rect(default, element.DesiredSize));

            RenderTargetBitmap rtb =
              new RenderTargetBitmap(
                (int)element.DesiredSize.Width,
                (int)element.DesiredSize.Height,
                96,
                96,
                PixelFormats.Pbgra32);

            rtb.Render(element);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));

            using (var ms = new MemoryStream())
            {
                encoder.Save(ms);
                using (var bmp = new System.Drawing.Bitmap(ms))
                    return InternalCreateCursor(bmp);
            }
        }

        private static Cursor InternalCreateCursor(System.Drawing.Bitmap bmp)
        {
            var iconInfo = default(NativeMethods.IconInfo);
            NativeMethods.GetIconInfo(bmp.GetHicon(), ref iconInfo);

            iconInfo.xHotspot = 0;
            iconInfo.yHotspot = 0;
            iconInfo.fIcon = false;

            SafeIconHandle cursorHandle = NativeMethods.CreateIconIndirect(ref iconInfo);
            return CursorInteropHelper.Create(cursorHandle);
        }

        private static class NativeMethods
        {
            [DllImport("user32.dll")]
            public static extern SafeIconHandle CreateIconIndirect(ref IconInfo icon);

            [DllImport("user32.dll")]
            public static extern bool DestroyIcon(IntPtr hIcon);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);

            public struct IconInfo
            {
                public bool fIcon;
                public int xHotspot;
                public int yHotspot;
                public IntPtr hbmMask;
                public IntPtr hbmColor;
            }
        }

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        private class SafeIconHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
            public SafeIconHandle()
                : base(true)
            {
            }

            protected override bool ReleaseHandle()
            {
                return NativeMethods.DestroyIcon(handle);
            }
        }
    }
}
