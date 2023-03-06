#pragma warning disable CS1591, SA1600
namespace RxBim.MsiBuilder.Bin
{
    using System;
    using CommandLine;
    using Figgle;
    using Microsoft.Win32;
    using Nuke;
    using WixSharp;

    public static class Program
    {
        public static int Main(string[] args)
        {
            Console.WriteLine(
                FiggleFonts.Slant.Render("RxBim Msi Builder"));

            try
            {
                CheckNetFramework();

                Parser.Default.ParseArguments<Options>(args)
                    .WithParsed(o =>
                    {
                        var wixInstaller = new WixInstaller();
                        var project = wixInstaller.CreateProject(o);
                        Compiler.BuildMsi(project);
                    });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 1;
            }

            return 0;
        }

        private static void CheckNetFramework()
        {
            var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5");
            if (key == null)
            {
                throw new ApplicationException(".NET Framework v3.5 not find on system!");
            }
        }
    }
}