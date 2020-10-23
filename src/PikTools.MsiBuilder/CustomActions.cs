namespace PikTools.MsiBuilder
{
    using Microsoft.Deployment.WindowsInstaller;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Management.Automation;
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
                var _powershell = PowerShell.Create().AddScript(script);
                _powershell.Invoke<User>();
            }

            return ActionResult.Success;
        }

        private static IEnumerable<string> FillFonts(string path)
        {
            foreach (var file in Directory.EnumerateFiles(path, "*.ttf", SearchOption.AllDirectories))
                yield return file;
        }

        public static bool IsFontInstalled(string fontPath)
        {
            var fontCol = new System.Drawing.Text.PrivateFontCollection();
            fontCol.AddFontFile(fontPath);
            var fontName = fontCol.Families.FirstOrDefault()?.Name;
            if (string.IsNullOrEmpty(fontName))
                return false;

            using (var testFont = new System.Drawing.Font(fontName, 8))
                return 0 == string.Compare(fontName, testFont.Name, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
