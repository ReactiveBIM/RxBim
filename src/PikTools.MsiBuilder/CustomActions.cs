#pragma warning disable CS1591, SA1600
namespace PikTools.MsiBuilder
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Management.Automation;
    using Microsoft.Deployment.WindowsInstaller;
    using WixSharp;

    public class CustomActions
    {
        [CustomAction]
        public static ActionResult InstallFonts(Session session)
        {
            var script =
                "$FONTS = 0x14"
                + Environment.NewLine
                + "$objShell = New-Object -ComObject Shell.Application"
                + Environment.NewLine
                + "$objFolder = $objShell.Namespace($FONTS)";

            if (FillFonts(session["INSTALLDIR"])
                .Where(fontPath =>
                {
                    if (IsFontInstalled(fontPath))
                        return false;

                    script +=
                        Environment.NewLine
                        + $"$objFolder.CopyHere(\"{fontPath}\")";
                    return true;
                }).ToArray().Any())
            {
                var powershell = PowerShell.Create().AddScript(script);
                powershell.Invoke<User>();
            }

            return ActionResult.Success;
        }

        public static bool IsFontInstalled(string fontPath)
        {
            var fontCol = new System.Drawing.Text.PrivateFontCollection();
            fontCol.AddFontFile(fontPath);
            var fontName = fontCol.Families.FirstOrDefault()?.Name;
            if (string.IsNullOrEmpty(fontName))
                return false;

            using (var testFont = new System.Drawing.Font(fontName, 8))
                return string.Compare(fontName, testFont.Name, StringComparison.InvariantCultureIgnoreCase) == 0;
        }

        private static IEnumerable<string> FillFonts(string path)
        {
            foreach (var file in Directory.EnumerateFiles(path, "*.ttf", SearchOption.AllDirectories))
                yield return file;
        }
    }
}