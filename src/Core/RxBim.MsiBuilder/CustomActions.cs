#pragma warning disable CS1591, SA1600
namespace RxBim.MsiBuilder
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Management.Automation;
    using System.Text;
    using Microsoft.Deployment.WindowsInstaller;
    using WixSharp;

    public class CustomActions
    {
        [CustomAction]
        public static ActionResult InstallFonts(Session session)
        {
            var notInstalledFontFiles = GetFontFiles(session["INSTALLDIR"])
                .Where(fontPath => !IsFontInstalled(fontPath))
                .ToList();

            if (!notInstalledFontFiles.Any())
                return ActionResult.Success;

            var scriptBuilder = new StringBuilder();
            scriptBuilder
                .Append("$FONTS = 0x14")
                .AppendLine("$objShell = New-Object -ComObject Shell.Application")
                .AppendLine("$objFolder = $objShell.Namespace($FONTS)");

            foreach (var fontPath in notInstalledFontFiles)
                scriptBuilder.AppendLine($"$objFolder.CopyHere(\"{fontPath}\")");

            var powershell = PowerShell.Create().AddScript(scriptBuilder.ToString());
            powershell.Invoke<User>();

            return ActionResult.Success;
        }

        private static bool IsFontInstalled(string fontPath)
        {
            var fontCol = new System.Drawing.Text.PrivateFontCollection();
            fontCol.AddFontFile(fontPath);
            var fontName = fontCol.Families.FirstOrDefault()?.Name;
            if (string.IsNullOrEmpty(fontName))
                return false;

            using var testFont = new System.Drawing.Font(fontName, 8);
            return string.Compare(fontName, testFont.Name, StringComparison.InvariantCultureIgnoreCase) == 0;
        }

        private static IEnumerable<string> GetFontFiles(string rootDirPath)
        {
            return Directory.EnumerateFiles(rootDirPath, "*.ttf", SearchOption.AllDirectories);
        }
    }
}