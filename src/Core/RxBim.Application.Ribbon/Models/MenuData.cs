namespace RxBim.Application.Ribbon
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Menu data.
    /// </summary>
    public class MenuData
    {
        private Assembly? _menuAssembly;

        /// <summary>
        /// Ribbon configuration
        /// </summary>
        public Ribbon? RibbonConfiguration { get; set; }

        /// <summary>
        /// Menu defining assembly.
        /// </summary>
        public Assembly MenuAssembly
        {
            get => _menuAssembly ?? throw new InvalidOperationException("No value set!");
            set => _menuAssembly = value;
        }

        /// <summary>
        /// Returns an image of the button's icon.
        /// </summary>
        /// <param name="resourcePath">The image resource path.</param>
        /// <param name="assembly">The assembly containing image embedded resource.</param>
        public ImageSource? GetIconImage(string? resourcePath, Assembly? assembly = null)
        {
            if (string.IsNullOrWhiteSpace(resourcePath))
                return null;

            assembly ??= MenuAssembly;

            var resource = assembly.GetManifestResourceNames()
                .FirstOrDefault(x => x.EndsWith(resourcePath!.Replace('\\', '.')));
            if (resource != null)
            {
                var file = assembly.GetManifestResourceStream(resource);
                if (file != null)
                {
                    var imageExtension = Path.GetExtension(resourcePath);
                    BitmapDecoder bd = imageExtension switch
                    {
                        ".png" => new PngBitmapDecoder(
                            file,
                            BitmapCreateOptions.PreservePixelFormat,
                            BitmapCacheOption.Default),
                        ".bmp" => new BmpBitmapDecoder(
                            file,
                            BitmapCreateOptions.PreservePixelFormat,
                            BitmapCacheOption.Default),
                        ".jpg" => new JpegBitmapDecoder(
                            file,
                            BitmapCreateOptions.PreservePixelFormat,
                            BitmapCacheOption.Default),
                        ".ico" => new IconBitmapDecoder(
                            file,
                            BitmapCreateOptions.PreservePixelFormat,
                            BitmapCacheOption.Default),
                        _ => throw new NotSupportedException($"Image with {imageExtension} extension is not supported.")
                    };
                    return bd.Frames[0];
                }
            }

            var uri = assembly.TryGetSupportFileUri(resourcePath!);
            return uri != null ? new BitmapImage(uri) : null;
        }

        /// <summary>
        /// Returns tooltip content for command button
        /// </summary>
        /// <param name="cmdButtonConfig">Command button configuration</param>
        /// <param name="commandType">Type of command class</param>
        public string? GetTooltipContent(CommandButton cmdButtonConfig, Type commandType)
        {
            var toolTip = cmdButtonConfig.ToolTip;
            if (RibbonConfiguration is null || toolTip is null || !RibbonConfiguration.DisplayVersion)
                return toolTip;
            if (toolTip.Length > 0)
                toolTip += Environment.NewLine;
            toolTip += $"{RibbonConfiguration.VersionPrefix}{commandType.Assembly.GetName().Version}";
            return toolTip;
        }
    }
}